using Biogenom.Nutrition.Domain.Entities;

namespace Biogenom.Nutrition.Application.Interfaces;

public interface INutrientBalanceService
{
    Task CreateNutrientBalancesAsync(NutritionAssessment assessment);
} 