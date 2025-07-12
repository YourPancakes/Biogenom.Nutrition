using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Application.Interfaces;
using Biogenom.Nutrition.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Biogenom.Nutrition.Application.Services;

public class NutritionScoreCalculator : INutritionScoreCalculator
{
    private readonly ILogger<NutritionScoreCalculator> _logger;

    public NutritionScoreCalculator(ILogger<NutritionScoreCalculator> logger)
    {
        _logger = logger;
    }

    public int CalculateScore(CreateAssessmentRequest request)
    {
        int score = 0;
        int meals = request.MealsPerDay ?? 0;
        int vegetables = request.VegetablesPerDay ?? 0;
        int fruits = request.FruitsPerDay ?? 0;
        int water = request.WaterIntake ?? 0;
        bool breakfast = request.EatsBreakfast ?? false;
        bool fastFood = request.EatsFastFood ?? false;
        bool processedFood = request.EatsProcessedFood ?? false;
        var activity = request.ActivityLevel ?? ActivityLevel.Sedentary;
        var sleep = request.SleepQuality ?? SleepQuality.Poor;
        var stress = request.StressLevel ?? StressLevel.High;

        score += meals switch
        {
            >= 3 => 10,
            2 => 5,
            _ => 0
        };

        score += vegetables switch
        {
            >= 5 => 15,
            >= 3 => 10,
            >= 1 => 5,
            _ => 0
        };

        score += fruits switch
        {
            >= 2 => 10,
            1 => 5,
            _ => 0
        };

        score += water switch
        {
            >= 8 => 10,
            >= 6 => 7,
            >= 4 => 5,
            _ => 0
        };

        if (breakfast) score += 10;
        if (!fastFood) score += 10;
        if (!processedFood) score += 10;

        score += activity switch
        {
            >= ActivityLevel.ModeratelyActive => 10,
            >= ActivityLevel.LightlyActive => 5,
            _ => 0
        };

        if (sleep >= SleepQuality.Good) score += 10;
        if (stress <= StressLevel.Moderate) score += 10;

        _logger.LogDebug("Calculated nutrition score: {Score} for assessment request", score);
        return score;
    }

    public NutritionQuality DetermineQualityLevel(int score)
    {
        var quality = score switch
        {
            >= 80 => NutritionQuality.Excellent,
            >= 60 => NutritionQuality.Good,
            >= 40 => NutritionQuality.Fair,
            _ => NutritionQuality.Poor
        };

        _logger.LogDebug("Determined nutrition quality level: {Quality} for score {Score}", quality, score);
        return quality;
    }

    public (int TotalScore, NutritionQuality QualityLevel) CalculateAssessmentScore(CreateAssessmentRequest request)
    {
        var totalScore = CalculateScore(request);
        var qualityLevel = DetermineQualityLevel(totalScore);

        _logger.LogInformation("Calculated assessment score: {Score} with quality {Quality}", totalScore, qualityLevel);
        return (totalScore, qualityLevel);
    }
} 