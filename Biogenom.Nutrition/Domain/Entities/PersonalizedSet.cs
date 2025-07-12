namespace Biogenom.Nutrition.Domain.Entities;

public class PersonalizedSet
{
    public int Id { get; set; }
    public int NutritionAssessmentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
    
    public virtual NutritionAssessment NutritionAssessment { get; set; } = null!;
    public virtual ICollection<Supplement> Supplements { get; set; } = new List<Supplement>();
} 