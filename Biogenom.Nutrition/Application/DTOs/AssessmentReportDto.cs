using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.DTOs;

public record AssessmentReportDto
{
    public int AssessmentId { get; init; }
    public DateTime CreatedAt { get; init; }
    
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public Gender Gender { get; init; }
    public decimal Weight { get; init; }
    public decimal Height { get; init; }
    
    public int MealsPerDay { get; init; }
    public int VegetablesPerDay { get; init; }
    public int FruitsPerDay { get; init; }
    public int WaterIntake { get; init; }
    public bool EatsBreakfast { get; init; }
    public bool EatsFastFood { get; init; }
    public bool EatsProcessedFood { get; init; }
    public ActivityLevel ActivityLevel { get; init; }
    public SleepQuality SleepQuality { get; init; }
    public StressLevel StressLevel { get; init; }
    
    public int TotalScore { get; init; }
    public NutritionQuality QualityLevel { get; init; }
    
    public List<ReportNutrientDto> NutrientBalances { get; init; } = new List<ReportNutrientDto>();
    public PersonalizedSetDto? PersonalizedSet { get; init; }
} 