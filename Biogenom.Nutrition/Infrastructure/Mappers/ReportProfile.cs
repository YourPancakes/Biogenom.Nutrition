using AutoMapper;
using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Domain.Entities;

namespace Biogenom.Nutrition.Infrastructure.Mappers;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<Nutrient, ReportNutrientDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
            .ForMember(dest => dest.DailyNormMin, opt => opt.MapFrom(src => src.DailyNormMin))
            .ForMember(dest => dest.DailyNormMax, opt => opt.MapFrom(src => src.DailyNormMax));

        CreateMap<NutrientBalance, ReportNutrientDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nutrient.Name))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Nutrient.Unit))
            .ForMember(dest => dest.DailyNormMin, opt => opt.MapFrom(src => src.Nutrient.DailyNormMin))
            .ForMember(dest => dest.DailyNormMax, opt => opt.MapFrom(src => src.Nutrient.DailyNormMax));

        CreateMap<Supplement, SupplementDto>();

        CreateMap<PersonalizedSet, PersonalizedSetDto>();
    }
} 