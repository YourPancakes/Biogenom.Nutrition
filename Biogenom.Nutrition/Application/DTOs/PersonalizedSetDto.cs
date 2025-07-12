using System.Text.Json.Serialization;

namespace Biogenom.Nutrition.Application.DTOs;

public class PersonalizedSetDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SupplementDto> Supplements { get; set; } = new List<SupplementDto>();
} 