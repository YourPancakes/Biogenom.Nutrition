namespace Biogenom.Nutrition.Domain.Entities;

public class Nutrient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal DailyNormMin { get; set; }
    public decimal DailyNormMax { get; set; }
} 