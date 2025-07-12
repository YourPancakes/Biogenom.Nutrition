using Biogenom.Nutrition.Domain.Entities;

namespace Biogenom.Nutrition.Application.Interfaces;

public interface INutritionAssessmentRepository
{
    Task<NutritionAssessment?> GetLatestAsync();
    Task<NutritionAssessment> CreateAsync(NutritionAssessment assessment);
    Task<NutritionAssessment> UpdateAsync(NutritionAssessment assessment);
    Task<bool> ExistsAsync();
} 