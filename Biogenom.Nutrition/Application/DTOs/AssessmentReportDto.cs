using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.DTOs;

public class AssessmentReportDto
{
    public int AssessmentId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    
    public int MealsPerDay { get; set; }
    public int VegetablesPerDay { get; set; }
    public int FruitsPerDay { get; set; }
    public int WaterIntake { get; set; }
    public bool EatsBreakfast { get; set; }
    public bool EatsFastFood { get; set; }
    public bool EatsProcessedFood { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public SleepQuality SleepQuality { get; set; }
    public StressLevel StressLevel { get; set; }
    
    public int TotalScore { get; set; }
    public NutritionQuality QualityLevel { get; set; }
    
    public List<ReportNutrientDto> NutrientBalances { get; set; } = new List<ReportNutrientDto>();
    public PersonalizedSetDto? PersonalizedSet { get; set; }
} 