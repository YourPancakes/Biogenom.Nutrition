using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Domain.Entities;

namespace Biogenom.Nutrition.Application.Interfaces;

public interface IPersonalizedSetService
{
    Task<PersonalizedSetDto> GetPersonalizedSetAsync();
    Task<PersonalizedSetDto> GetPersonalizedSetByIdAsync(int assessmentId);
    Task CreatePersonalizedSetAsync(NutritionAssessment assessment);
} 