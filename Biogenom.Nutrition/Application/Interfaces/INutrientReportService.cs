using Biogenom.Nutrition.Application.DTOs;

namespace Biogenom.Nutrition.Application.Interfaces;

public interface INutrientReportService
{
    Task<List<ReportNutrientDto>> GetCurrentNutrientReportAsync();
    Task<List<ReportNutrientDto>> GetNutrientReportByIdAsync(int assessmentId);
} 