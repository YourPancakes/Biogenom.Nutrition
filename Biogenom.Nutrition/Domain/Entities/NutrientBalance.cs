using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Domain.Entities;

public class NutrientBalance
{
    public int Id { get; set; }
    public int NutritionAssessmentId { get; set; }
    public int NutrientId { get; set; }
    
    public decimal CurrentValue { get; set; }
    public decimal FromDiet { get; set; }
    public decimal FromSupplements { get; set; }
    public NutrientStatus Status { get; set; }
    
    public virtual NutritionAssessment NutritionAssessment { get; set; } = null!;
    public virtual Nutrient Nutrient { get; set; } = null!;
} 