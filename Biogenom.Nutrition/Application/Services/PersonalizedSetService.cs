using AutoMapper;
using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Application.Interfaces;
using Biogenom.Nutrition.Domain.Entities;
using Biogenom.Nutrition.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Biogenom.Nutrition.Persistence.Context;

namespace Biogenom.Nutrition.Application.Services;

public class PersonalizedSetService : IPersonalizedSetService
{
    private readonly IRepository<NutritionAssessment> _assessmentRepository;
    private readonly IRepository<PersonalizedSet> _personalizedSetRepository;
    private readonly IRepository<Supplement> _supplementRepository;
    private readonly INutritionScoreCalculator _scoreCalculator;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonalizedSetService> _logger;

    public PersonalizedSetService(
        IRepository<NutritionAssessment> assessmentRepository,
        IRepository<PersonalizedSet> personalizedSetRepository,
        IRepository<Supplement> supplementRepository,
        INutritionScoreCalculator scoreCalculator,
        IMapper mapper,
        ILogger<PersonalizedSetService> logger)
    {
        _assessmentRepository = assessmentRepository;
        _personalizedSetRepository = personalizedSetRepository;
        _supplementRepository = supplementRepository;
        _scoreCalculator = scoreCalculator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PersonalizedSetDto> GetPersonalizedSetAsync()
    {
        try
        {
            var assessments = await _assessmentRepository.GetAllAsync();
            var latestAssessment = assessments.OrderByDescending(a => a.CreatedAt).FirstOrDefault();

            if (latestAssessment == null)
            {
                _logger.LogWarning("No nutrition assessment found");
                return new PersonalizedSetDto
                {
                    Id = 0,
                    CreatedAt = DateTime.UtcNow,
                    Name = string.Empty,
                    Description = string.Empty,
                    Supplements = new List<SupplementDto>()
                };
            }

            var context = _personalizedSetRepository.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_personalizedSetRepository) as ApplicationDbContext;
            
            PersonalizedSet? personalizedSet = null;
            if (context != null)
            {
                personalizedSet = await context.PersonalizedSets
                    .Include(ps => ps.Supplements)
                    .FirstOrDefaultAsync(s => s.NutritionAssessmentId == latestAssessment.Id);
            }

            if (personalizedSet == null)
            {
                _logger.LogWarning("No personalized set found for assessment {AssessmentId}", latestAssessment.Id);
                return new PersonalizedSetDto
                {
                    Id = 0,
                    CreatedAt = DateTime.UtcNow,
                    Name = string.Empty,
                    Description = string.Empty,
                    Supplements = new List<SupplementDto>()
                };
            }

            var supplementDtos = _mapper.Map<List<SupplementDto>>(personalizedSet.Supplements);

            var result = _mapper.Map<PersonalizedSetDto>(personalizedSet) with { Supplements = supplementDtos };

            _logger.LogInformation("Retrieved personalized set with {Count} supplements", supplementDtos.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving personalized set");
            throw;
        }
    }

    public async Task<PersonalizedSetDto> GetPersonalizedSetByIdAsync(int assessmentId)
    {
        try
        {
            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId);

            if (assessment == null)
            {
                _logger.LogWarning("Assessment with id {AssessmentId} not found", assessmentId);
                return new PersonalizedSetDto
                {
                    Id = 0,
                    CreatedAt = DateTime.UtcNow,
                    Name = string.Empty,
                    Description = string.Empty,
                    Supplements = new List<SupplementDto>()
                };
            }

            var context = _personalizedSetRepository.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_personalizedSetRepository) as ApplicationDbContext;
            
            PersonalizedSet? personalizedSet = null;
            if (context != null)
            {
                personalizedSet = await context.PersonalizedSets
                    .Include(ps => ps.Supplements)
                    .FirstOrDefaultAsync(s => s.NutritionAssessmentId == assessmentId);
            }

            if (personalizedSet == null)
            {
                _logger.LogWarning("No personalized set found for assessment {AssessmentId}", assessmentId);
                return new PersonalizedSetDto
                {
                    Id = 0,
                    CreatedAt = DateTime.UtcNow,
                    Name = string.Empty,
                    Description = string.Empty,
                    Supplements = new List<SupplementDto>()
                };
            }

            var supplementDtos = _mapper.Map<List<SupplementDto>>(personalizedSet.Supplements);

            var result = _mapper.Map<PersonalizedSetDto>(personalizedSet) with { Supplements = supplementDtos };

            _logger.LogInformation("Retrieved personalized set for assessment {AssessmentId} with {Count} supplements", assessmentId, supplementDtos.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving personalized set for assessment {AssessmentId}", assessmentId);
            throw;
        }
    }

    public async Task CreatePersonalizedSetAsync(NutritionAssessment assessment)
    {
        try
        {
            var supplements = await _supplementRepository.GetAllAsync();
            var recommendedSupplements = SelectSupplementsForAssessment(assessment, supplements);
            
            var personalizedSet = new PersonalizedSet
            {
                NutritionAssessmentId = assessment.Id,
                CreatedAt = DateTime.UtcNow,
                Name = GenerateSetName(assessment),
                Description = GenerateSetDescription(assessment)
            };
            
            var createdSet = await _personalizedSetRepository.AddAsync(personalizedSet);
            
            var context = _personalizedSetRepository.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_personalizedSetRepository) as ApplicationDbContext;
            
            if (context != null)
            {
                var setWithSupplements = await context.PersonalizedSets
                    .Include(ps => ps.Supplements)
                    .FirstOrDefaultAsync(ps => ps.Id == createdSet.Id);
                
                if (setWithSupplements != null)
                {
                    setWithSupplements.Supplements = recommendedSupplements;
                    await context.SaveChangesAsync();
                }
            }
            
            _logger.LogInformation("Created personalized set with {Count} supplements for assessment {AssessmentId}", 
                recommendedSupplements.Count, assessment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating personalized set for assessment {AssessmentId}", assessment.Id);
            throw;
        }
    }

    private List<Supplement> SelectSupplementsForAssessment(NutritionAssessment assessment, IEnumerable<Supplement> allSupplements)
    {
        var selectedSupplements = new List<Supplement>();
        
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
        
        foreach (var supplement in allSupplements)
        {
            var shouldInclude = supplement.Name.ToLower() switch
            {
                var name when name.Contains("vitamin d") => qualityLevel <= NutritionQuality.Good,
                var name when name.Contains("omega-3") => !assessment.EatsFastFood && assessment.ActivityLevel >= ActivityLevel.ModeratelyActive,
                var name when name.Contains("vitamin c") => assessment.VegetablesPerDay < 5 || assessment.FruitsPerDay < 2,
                var name when name.Contains("calcium") => assessment.Age > 30 || assessment.Gender == Gender.Female,
                var name when name.Contains("magnesium") => assessment.StressLevel == StressLevel.High || assessment.SleepQuality <= SleepQuality.Fair,
                var name when name.Contains("zinc") => assessment.ActivityLevel >= ActivityLevel.VeryActive,
                var name when name.Contains("probiotic") => assessment.EatsProcessedFood || assessment.EatsFastFood,
                _ => qualityLevel <= NutritionQuality.Fair
            };
            
            if (shouldInclude)
            {
                selectedSupplements.Add(supplement);
            }
        }
        
        return selectedSupplements;
    }

    private string GenerateSetName(NutritionAssessment assessment)
    {
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
        
        return qualityLevel switch
        {
            NutritionQuality.Excellent => $"Excellent Nutrition Set - {assessment.Name}",
            NutritionQuality.Good => $"Nutrition Improvement Set - {assessment.Name}",
            NutritionQuality.Fair => $"Basic Support Set - {assessment.Name}",
            NutritionQuality.Poor => $"Intensive Recovery Set - {assessment.Name}",
            _ => $"Personalized Set - {assessment.Name}"
        };
    }

    private string GenerateSetDescription(NutritionAssessment assessment)
    {
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
        
        var (totalScore, qualityLevel) = _scoreCalculator.CalculateAssessmentScore(request);
        
        return $"Personalized supplement set selected based on your nutrition assessment. " +
               $"Nutrition quality: {qualityLevel}, Total score: {totalScore}/100. " +
               $"Recommended for {assessment.ActivityLevel} activity level.";
    }
} 