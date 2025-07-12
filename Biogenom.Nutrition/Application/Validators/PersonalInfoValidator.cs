using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Domain.Enums;
using FluentValidation;

namespace Biogenom.Nutrition.Application.Validators;

public class PersonalInfoValidator : AbstractValidator<CreateAssessmentRequest>
{
    public PersonalInfoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Age)
            .InclusiveBetween(1, 120)
            .When(x => x.Age.HasValue)
            .WithMessage("Age must be between 1 and 120");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue)
            .WithMessage("Invalid gender value");

        RuleFor(x => x.Weight)
            .InclusiveBetween(20, 300)
            .When(x => x.Weight.HasValue)
            .WithMessage("Weight must be between 20 and 300 kg");

        RuleFor(x => x.Height)
            .InclusiveBetween(100, 250)
            .When(x => x.Height.HasValue)
            .WithMessage("Height must be between 100 and 250 cm");
    }
} 