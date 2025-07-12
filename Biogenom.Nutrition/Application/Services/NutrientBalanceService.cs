using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Application.Interfaces;
using Biogenom.Nutrition.Domain.Entities;
using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.Services;

public class NutrientBalanceService : INutrientBalanceService
{
    private readonly IRepository<Nutrient> _nutrientRepository;
    private readonly IRepository<NutrientBalance> _nutrientBalanceRepository;
    private readonly INutritionScoreCalculator _scoreCalculator;
    private readonly ILogger<NutrientBalanceService> _logger;

    public NutrientBalanceService(
        IRepository<Nutrient> nutrientRepository,
        IRepository<NutrientBalance> nutrientBalanceRepository,
        INutritionScoreCalculator scoreCalculator,
        ILogger<NutrientBalanceService> logger)
    {
        _nutrientRepository = nutrientRepository;
        _nutrientBalanceRepository = nutrientBalanceRepository;
        _scoreCalculator = scoreCalculator;
        _logger = logger;
    }

    public async Task CreateNutrientBalancesAsync(NutritionAssessment assessment)
    {
        try
        {
            var nutrients = await _nutrientRepository.GetAllAsync();
            
            foreach (var nutrient in nutrients)
            {
                var (fromDiet, fromSupplements, status) = CalculateNutrientValues(nutrient.Name, assessment);
                
                var nutrientBalance = new NutrientBalance
                {
                    NutritionAssessmentId = assessment.Id,
                    NutrientId = nutrient.Id,
                    CurrentValue = fromDiet + fromSupplements,
                    FromDiet = fromDiet,
                    FromSupplements = fromSupplements,
                    Status = status
                };
                
                await _nutrientBalanceRepository.AddAsync(nutrientBalance);
            }
            
            _logger.LogInformation("Created nutrient balances for assessment {AssessmentId}", assessment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating nutrient balances for assessment {AssessmentId}", assessment.Id);
            throw;
        }
    }

    private (decimal fromDiet, decimal fromSupplements, NutrientStatus status) CalculateNutrientValues(string nutrientName, NutritionAssessment assessment)
    {
        var fromDiet = CalculateDietValue(nutrientName, assessment);

        var request = new CreateAssessmentRequest
        {
            Name = assessment.Name,
            Age = assessment.Age,
            Gender = assessment.Gender,
            Weight = assessment.Weight,
            Height = assessment.Height,
            MealsPerDay = assessment.MealsPerDay,
            VegetablesPerDay = assessment.VegetablesPerDay,
            FruitsPerDay = assessment.FruitsPerDay,
            WaterIntake = assessment.WaterIntake,
            EatsBreakfast = assessment.EatsBreakfast,
            EatsFastFood = assessment.EatsFastFood,
            EatsProcessedFood = assessment.EatsProcessedFood,
            ActivityLevel = assessment.ActivityLevel,
            SleepQuality = assessment.SleepQuality,
            StressLevel = assessment.StressLevel
        };
        
        var (_, qualityLevel) = _scoreCalculator.CalculateAssessmentScore(request);

        var fromSupplements = qualityLevel switch
        {
            NutritionQuality.Poor => 5.0m,
            NutritionQuality.Fair => 3.0m,
            NutritionQuality.Good => 1.5m,
            NutritionQuality.Excellent => 0.5m,
            _ => 2.0m
        };

        var total = fromDiet + fromSupplements;
        var status = total switch
        {
            >= 8.0m => NutrientStatus.Normal,
            >= 5.0m => NutrientStatus.Deficit,
            _ => NutrientStatus.Deficit
        };

        return (fromDiet, fromSupplements, status);
    }

    private decimal CalculateDietValue(string nutrientName, NutritionAssessment assessment)
    {
        return nutrientName.ToLower() switch
        {
            "vitamin d" => assessment.VegetablesPerDay * 0.5m + (assessment.EatsBreakfast ? 2.0m : 0m),
            "omega-3" => assessment.FruitsPerDay * 0.3m + (assessment.EatsFastFood ? 0m : 1.0m),
            "vitamin c" => assessment.VegetablesPerDay * 2.0m + assessment.FruitsPerDay * 3.0m,
            "calcium" => assessment.MealsPerDay * 1.5m + (assessment.EatsBreakfast ? 3.0m : 0m),
            "iron" => assessment.VegetablesPerDay * 0.8m + assessment.FruitsPerDay * 0.5m,
            "magnesium" => assessment.MealsPerDay * 2.0m + assessment.WaterIntake * 0.2m,
            "zinc" => assessment.MealsPerDay * 1.0m + (assessment.ActivityLevel >= ActivityLevel.ModeratelyActive ? 2.0m : 0m),
            "vitamin b12" => assessment.MealsPerDay * 1.2m + (assessment.EatsProcessedFood ? 0m : 1.0m),
            _ => assessment.MealsPerDay * 1.0m
        };
    }
} 