using AutoMapper;
using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Domain.Entities;

namespace Biogenom.Nutrition.Infrastructure.Mappers;

public class NutritionAssessmentProfile : Profile
{
    public NutritionAssessmentProfile()
    {
        CreateMap<CreateAssessmentRequest, NutritionAssessment>();
        
        CreateMap<NutritionAssessment, NutritionAssessmentDto>();
        
        CreateMap<NutritionAssessment, AssessmentReportDto>()
            .ForMember(dest => dest.AssessmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.MealsPerDay, opt => opt.MapFrom(src => src.MealsPerDay))
            .ForMember(dest => dest.VegetablesPerDay, opt => opt.MapFrom(src => src.VegetablesPerDay))
            .ForMember(dest => dest.FruitsPerDay, opt => opt.MapFrom(src => src.FruitsPerDay))
            .ForMember(dest => dest.WaterIntake, opt => opt.MapFrom(src => src.WaterIntake))
            .ForMember(dest => dest.EatsBreakfast, opt => opt.MapFrom(src => src.EatsBreakfast))
            .ForMember(dest => dest.EatsFastFood, opt => opt.MapFrom(src => src.EatsFastFood))
            .ForMember(dest => dest.EatsProcessedFood, opt => opt.MapFrom(src => src.EatsProcessedFood))
            .ForMember(dest => dest.ActivityLevel, opt => opt.MapFrom(src => src.ActivityLevel))
            .ForMember(dest => dest.SleepQuality, opt => opt.MapFrom(src => src.SleepQuality))
            .ForMember(dest => dest.StressLevel, opt => opt.MapFrom(src => src.StressLevel))
            .ForMember(dest => dest.TotalScore, opt => opt.Ignore())
            .ForMember(dest => dest.QualityLevel, opt => opt.Ignore())
            .ForMember(dest => dest.NutrientBalances, opt => opt.MapFrom(src => src.NutrientBalances))
            .ForMember(dest => dest.PersonalizedSet, opt => opt.MapFrom(src => src.PersonalizedSets.FirstOrDefault()));
    }
} 