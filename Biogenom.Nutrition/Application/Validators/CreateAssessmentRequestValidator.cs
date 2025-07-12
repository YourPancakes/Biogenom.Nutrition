using Biogenom.Nutrition.Application.DTOs;
using FluentValidation;

namespace Biogenom.Nutrition.Application.Validators;

public class CreateAssessmentRequestValidator : AbstractValidator<CreateAssessmentRequest>
{
    public CreateAssessmentRequestValidator(
        PersonalInfoValidator personalInfoValidator,
        DietInfoValidator dietInfoValidator,
        LifestyleValidator lifestyleValidator)
    {
        Include(personalInfoValidator);
        Include(dietInfoValidator);
        Include(lifestyleValidator);
    }
} 