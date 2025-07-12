using AutoMapper;
using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Application.Interfaces;
using Biogenom.Nutrition.Domain.Entities;
using Biogenom.Nutrition.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Biogenom.Nutrition.Application.Services;

public class NutrientReportService : INutrientReportService
{
    private readonly IRepository<Nutrient> _nutrientRepository;
    private readonly IRepository<NutritionAssessment> _assessmentRepository;
    private readonly IRepository<NutrientBalance> _nutrientBalanceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<NutrientReportService> _logger;

    public NutrientReportService(
        IRepository<Nutrient> nutrientRepository,
        IRepository<NutritionAssessment> assessmentRepository,
        IRepository<NutrientBalance> nutrientBalanceRepository,
        IMapper mapper,
        ILogger<NutrientReportService> logger)
    {
        _nutrientRepository = nutrientRepository;
        _assessmentRepository = assessmentRepository;
        _nutrientBalanceRepository = nutrientBalanceRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ReportNutrientDto>> GetCurrentNutrientReportAsync()
    {
        try
        {
            var assessments = await _assessmentRepository.GetAllAsync();
            var latestAssessment = assessments.OrderByDescending(a => a.CreatedAt).FirstOrDefault();

            if (latestAssessment == null)
            {
                _logger.LogWarning("No nutrition assessment found");
                return new List<ReportNutrientDto>();
            }

            var nutrients = await _nutrientRepository.GetAllAsync();
            var nutrientBalances = await _nutrientBalanceRepository.GetAllAsync();

            var reportNutrients = new List<ReportNutrientDto>();

            foreach (var nutrient in nutrients)
            {
                var balance = nutrientBalances.FirstOrDefault(b => b.NutrientId == nutrient.Id && b.NutritionAssessmentId == latestAssessment.Id);
                
                var reportNutrient = _mapper.Map<ReportNutrientDto>(nutrient);
                
                if (balance != null)
                {
                    reportNutrient = reportNutrient with 
                    { 
                        CurrentValue = balance.CurrentValue,
                        FromDiet = balance.FromDiet,
                        FromSupplements = balance.FromSupplements,
                        Status = balance.Status
                    };
                }
                else
                {
                    reportNutrient = reportNutrient with 
                    { 
                        CurrentValue = 0,
                        FromDiet = 0,
                        FromSupplements = 0,
                        Status = NutrientStatus.Deficit
                    };
                }

                reportNutrients.Add(reportNutrient);
            }

            _logger.LogInformation("Retrieved current nutrient report with {Count} nutrients", reportNutrients.Count);
            return reportNutrients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current nutrient report");
            throw;
        }
    }

    public async Task<List<ReportNutrientDto>> GetNutrientReportByIdAsync(int assessmentId)
    {
        try
        {
            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId);

            if (assessment == null)
            {
                _logger.LogWarning("Assessment with id {AssessmentId} not found", assessmentId);
                return new List<ReportNutrientDto>();
            }

            var nutrients = await _nutrientRepository.GetAllAsync();
            var nutrientBalances = await _nutrientBalanceRepository.GetAllAsync();

            var reportNutrients = new List<ReportNutrientDto>();

            foreach (var nutrient in nutrients)
            {
                var balance = nutrientBalances.FirstOrDefault(b => b.NutrientId == nutrient.Id && b.NutritionAssessmentId == assessmentId);
                
                var reportNutrient = _mapper.Map<ReportNutrientDto>(nutrient);
                
                if (balance != null)
                {
                    reportNutrient = reportNutrient with 
                    { 
                        CurrentValue = balance.CurrentValue,
                        FromDiet = balance.FromDiet,
                        FromSupplements = balance.FromSupplements,
                        Status = balance.Status
                    };
                }
                else
                {
                    reportNutrient = reportNutrient with 
                    { 
                        CurrentValue = 0,
                        FromDiet = 0,
                        FromSupplements = 0,
                        Status = NutrientStatus.Deficit
                    };
                }

                reportNutrients.Add(reportNutrient);
            }

            _logger.LogInformation("Retrieved nutrient report for assessment {AssessmentId} with {Count} nutrients", assessmentId, reportNutrients.Count);
            return reportNutrients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving nutrient report for assessment {AssessmentId}", assessmentId);
            throw;
        }
    }
} 