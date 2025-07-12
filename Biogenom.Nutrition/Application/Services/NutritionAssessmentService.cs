using AutoMapper;
using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Application.Interfaces;
using Biogenom.Nutrition.Domain.Entities;
using Biogenom.Nutrition.Domain.Enums;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Biogenom.Nutrition.Application.Services;

public class NutritionAssessmentService : INutritionAssessmentService
{
    private readonly IRepository<NutritionAssessment> _assessmentRepository;
    private readonly INutrientBalanceService _nutrientBalanceService;
    private readonly IPersonalizedSetService _personalizedSetService;
    private readonly INutritionScoreCalculator _scoreCalculator;
    private readonly IMapper _mapper;
    private readonly ILogger<NutritionAssessmentService> _logger;

    public NutritionAssessmentService(
        IRepository<NutritionAssessment> assessmentRepository,
        INutrientBalanceService nutrientBalanceService,
        IPersonalizedSetService personalizedSetService,
        INutritionScoreCalculator scoreCalculator,
        IMapper mapper,
        ILogger<NutritionAssessmentService> logger)
    {
        _assessmentRepository = assessmentRepository;
        _nutrientBalanceService = nutrientBalanceService;
        _personalizedSetService = personalizedSetService;
        _scoreCalculator = scoreCalculator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<NutritionAssessmentDto>> CreateAssessmentAsync(CreateAssessmentRequest request)
    {
        try
        {
            var assessment = new NutritionAssessment
            {
                CreatedAt = DateTime.UtcNow,
                Name = request.Name ?? string.Empty,
                Age = request.Age ?? 0,
                Gender = request.Gender ?? Gender.Other,
                Weight = request.Weight ?? 0,
                Height = request.Height ?? 0,
                MealsPerDay = request.MealsPerDay ?? 0,
                VegetablesPerDay = request.VegetablesPerDay ?? 0,
                FruitsPerDay = request.FruitsPerDay ?? 0,
                WaterIntake = request.WaterIntake ?? 0,
                EatsBreakfast = request.EatsBreakfast ?? false,
                EatsFastFood = request.EatsFastFood ?? false,
                EatsProcessedFood = request.EatsProcessedFood ?? false,
                ActivityLevel = request.ActivityLevel ?? ActivityLevel.Sedentary,
                SleepQuality = request.SleepQuality ?? SleepQuality.Poor,
                StressLevel = request.StressLevel ?? StressLevel.High
            };

            var createdAssessment = await _assessmentRepository.AddAsync(assessment);
            
            await _nutrientBalanceService.CreateNutrientBalancesAsync(createdAssessment);
            await _personalizedSetService.CreatePersonalizedSetAsync(createdAssessment);
            
            var assessmentDto = _mapper.Map<NutritionAssessmentDto>(createdAssessment);

            _logger.LogInformation("Created nutrition assessment with ID {AssessmentId}", createdAssessment.Id);

            return Result.Ok(assessmentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating nutrition assessment");
            return Result.Fail<NutritionAssessmentDto>("Failed to create assessment");
        }
    }



    public async Task<Result<AssessmentReportDto>> GetAssessmentReportByIdAsync(int assessmentId)
    {
        try
        {
            var assessment = await _assessmentRepository.GetByIdWithIncludesAsync(
                assessmentId, 
                "NutrientBalances.Nutrient", 
                "PersonalizedSets.Supplements");

            if (assessment == null)
            {
                _logger.LogWarning("Assessment with id {AssessmentId} not found", assessmentId);
                return Result.Fail<AssessmentReportDto>($"Assessment with id {assessmentId} not found");
            }

            var reportDto = _mapper.Map<AssessmentReportDto>(assessment);
            
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
            reportDto.TotalScore = totalScore;
            reportDto.QualityLevel = qualityLevel;
            
            _logger.LogInformation("Successfully retrieved full assessment report for id {AssessmentId} with score {Score} and quality {Quality}", 
                assessmentId, totalScore, qualityLevel);
            return Result.Ok(reportDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving assessment report for id {AssessmentId}", assessmentId);
            return Result.Fail<AssessmentReportDto>("Failed to retrieve assessment report");
        }
    }

    public async Task<Result<IEnumerable<NutritionAssessmentDto>>> GetAllAssessmentsAsync()
    {
        try
        {
            var assessments = await _assessmentRepository.GetAllAsync();
            var assessmentDtos = _mapper.Map<IEnumerable<NutritionAssessmentDto>>(assessments);
            
            _logger.LogInformation("Retrieved {Count} assessments", assessments.Count());
            return Result.Ok(assessmentDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all assessments");
            return Result.Fail<IEnumerable<NutritionAssessmentDto>>("Failed to retrieve assessments");
        }
    }

    public async Task<Result> DeleteAssessmentAsync(int assessmentId)
    {
        try
        {
            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId);

            if (assessment == null)
            {
                _logger.LogWarning("Attempted to delete assessment with id {AssessmentId} that does not exist", assessmentId);
                return Result.Fail($"Assessment with id {assessmentId} not found");
            }

            await _assessmentRepository.DeleteAsync(assessment);
            
            _logger.LogInformation("Successfully deleted assessment with id {AssessmentId}", assessmentId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting assessment with id {AssessmentId}", assessmentId);
            return Result.Fail("Failed to delete assessment");
        }
    }




} 