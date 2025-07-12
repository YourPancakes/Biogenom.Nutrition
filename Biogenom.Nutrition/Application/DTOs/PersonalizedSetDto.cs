using System.Text.Json.Serialization;

namespace Biogenom.Nutrition.Application.DTOs;

public record PersonalizedSetDto
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<SupplementDto> Supplements { get; init; } = new List<SupplementDto>();
} 