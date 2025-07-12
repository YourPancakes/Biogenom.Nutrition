using Biogenom.Nutrition.Application.DTOs;
using FluentValidation;

namespace Biogenom.Nutrition.Application.Validators;

public class DietInfoValidator : AbstractValidator<CreateAssessmentRequest>
{
    public DietInfoValidator()
    {
        RuleFor(x => x.MealsPerDay)
            .InclusiveBetween(1, 10)
            .When(x => x.MealsPerDay.HasValue)
            .WithMessage("Meals per day must be between 1 and 10");

        RuleFor(x => x.VegetablesPerDay)
            .InclusiveBetween(0, 20)
            .When(x => x.VegetablesPerDay.HasValue)
            .WithMessage("Vegetables per day must be between 0 and 20");

        RuleFor(x => x.FruitsPerDay)
            .InclusiveBetween(0, 15)
            .When(x => x.FruitsPerDay.HasValue)
            .WithMessage("Fruits per day must be between 0 and 15");

        RuleFor(x => x.WaterIntake)
            .InclusiveBetween(0, 20)
            .When(x => x.WaterIntake.HasValue)
            .WithMessage("Water intake must be between 0 and 20 glasses");

        RuleFor(x => x.FoodAllergies)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.FoodAllergies))
            .WithMessage("Food allergies description cannot exceed 500 characters");

        RuleFor(x => x.Supplements)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Supplements))
            .WithMessage("Supplements description cannot exceed 500 characters");
    }
} 