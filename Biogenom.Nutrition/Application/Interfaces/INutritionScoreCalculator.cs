using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.Interfaces;

public interface INutritionScoreCalculator
{
    int CalculateScore(CreateAssessmentRequest request);
    NutritionQuality DetermineQualityLevel(int score);
    (int TotalScore, NutritionQuality QualityLevel) CalculateAssessmentScore(CreateAssessmentRequest request);
} 