using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Application.DTOs;

public record ReportNutrientDto
{
    public string Name { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public decimal CurrentValue { get; init; }
    public decimal FromDiet { get; init; }
    public decimal FromSupplements { get; init; }
    public decimal DailyNormMin { get; init; }
    public decimal DailyNormMax { get; init; }
    public NutrientStatus Status { get; init; }
} 