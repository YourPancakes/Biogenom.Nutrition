namespace Biogenom.Nutrition.Domain.Entities;

public class Supplement
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string WhenToTake { get; set; } = string.Empty;
    
    public virtual ICollection<PersonalizedSet> PersonalizedSets { get; set; } = new List<PersonalizedSet>();
} 