using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.DTOs;

public class ReportNutrientDto
{
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal FromDiet { get; set; }
    public decimal FromSupplements { get; set; }
    public decimal DailyNormMin { get; set; }
    public decimal DailyNormMax { get; set; }
    public NutrientStatus Status { get; set; }
} 