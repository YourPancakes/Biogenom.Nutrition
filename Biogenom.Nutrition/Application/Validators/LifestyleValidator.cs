using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Domain.Enums;
using FluentValidation;

namespace Biogenom.Nutrition.Application.Validators;

public class LifestyleValidator : AbstractValidator<CreateAssessmentRequest>
{
    public LifestyleValidator()
    {
        RuleFor(x => x.ActivityLevel)
            .IsInEnum()
            .When(x => x.ActivityLevel.HasValue)
            .WithMessage("Invalid activity level value");

        RuleFor(x => x.SleepQuality)
            .IsInEnum()
            .When(x => x.SleepQuality.HasValue)
            .WithMessage("Invalid sleep quality value");

        RuleFor(x => x.StressLevel)
            .IsInEnum()
            .When(x => x.StressLevel.HasValue)
            .WithMessage("Invalid stress level value");
    }
} 