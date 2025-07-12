using Biogenom.Nutrition.Application.DTOs;
using FluentResults;

namespace Biogenom.Nutrition.Application.Interfaces;

public interface INutritionAssessmentService
{
    Task<Result<NutritionAssessmentDto>> CreateAssessmentAsync(CreateAssessmentRequest request);
    Task<Result<AssessmentReportDto>> GetAssessmentReportByIdAsync(int assessmentId);
    Task<Result<IEnumerable<NutritionAssessmentDto>>> GetAllAssessmentsAsync();
    Task<Result> DeleteAssessmentAsync(int assessmentId);
} 