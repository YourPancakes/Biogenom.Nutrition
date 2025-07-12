using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.DTOs;

public record CreateAssessmentRequest
{
    public string? Name { get; init; }
    public int? Age { get; init; }
    public Gender? Gender { get; init; }
    public decimal? Weight { get; init; }
    public decimal? Height { get; init; }
    public int? MealsPerDay { get; init; }
    public int? VegetablesPerDay { get; init; }
    public int? FruitsPerDay { get; init; }
    public int? WaterIntake { get; init; }
    public bool? EatsBreakfast { get; init; }
    public bool? EatsFastFood { get; init; }
    public bool? EatsProcessedFood { get; init; }
    public bool? HasFoodAllergies { get; init; }
    public string? FoodAllergies { get; init; }
    public bool? TakesSupplements { get; init; }
    public string? Supplements { get; init; }
    public ActivityLevel? ActivityLevel { get; init; }
    public SleepQuality? SleepQuality { get; init; }
    public StressLevel? StressLevel { get; init; }
} 