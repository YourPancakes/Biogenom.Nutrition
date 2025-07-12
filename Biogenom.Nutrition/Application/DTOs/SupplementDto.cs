namespace Biogenom.Nutrition.Application.DTOs;

public record SupplementDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public string Dosage { get; init; } = string.Empty;
    public string WhenToTake { get; init; } = string.Empty;
} 