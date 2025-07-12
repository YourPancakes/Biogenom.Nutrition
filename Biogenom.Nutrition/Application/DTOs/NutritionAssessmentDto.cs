using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.DTOs;

public class NutritionAssessmentDto
{
    public int Id { get; set; }
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

} 