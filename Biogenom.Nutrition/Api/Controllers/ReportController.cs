using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Biogenom.Nutrition.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/report")]
public class ReportController : ControllerBase
{
    private readonly INutrientReportService _nutrientReportService;
    private readonly IPersonalizedSetService _personalizedSetService;
    private readonly ILogger<ReportController> _logger;

    public ReportController(
        INutrientReportService nutrientReportService,
        IPersonalizedSetService personalizedSetService,
        ILogger<ReportController> logger)
    {
        _nutrientReportService = nutrientReportService;
        _personalizedSetService = personalizedSetService;
        _logger = logger;
    }

    [HttpGet("current/last")]
    [ProducesResponseType(typeof(List<ReportNutrientDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<ReportNutrientDto>>> GetCurrentNutrientReport()
    {
        try
        {
            var report = await _nutrientReportService.GetCurrentNutrientReportAsync();
            
            if (!report.Any())
            {
                return NotFound("No nutrient data found");
            }

            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current nutrient report");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("current/{assessmentId}")]
    [ProducesResponseType(typeof(List<ReportNutrientDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<ReportNutrientDto>>> GetNutrientReportById(int assessmentId)
    {
        try
        {
            var report = await _nutrientReportService.GetNutrientReportByIdAsync(assessmentId);
            
            if (!report.Any())
            {
                return NotFound("No nutrient data found for this assessment");
            }

            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting nutrient report for assessment {AssessmentId}", assessmentId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("personalized-set/last")]
    [ProducesResponseType(typeof(PersonalizedSetDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PersonalizedSetDto>> GetPersonalizedSet()
    {
        try
        {
            var personalizedSet = await _personalizedSetService.GetPersonalizedSetAsync();
            
            if (personalizedSet.Supplements.Count == 0)
            {
                return NotFound("No personalized set found");
            }

            return Ok(personalizedSet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting personalized set");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("personalized-set/{assessmentId}")]
    [ProducesResponseType(typeof(PersonalizedSetDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PersonalizedSetDto>> GetPersonalizedSetById(int assessmentId)
    {
        try
        {
            var personalizedSet = await _personalizedSetService.GetPersonalizedSetByIdAsync(assessmentId);
            
            if (personalizedSet.Supplements.Count == 0)
            {
                return NotFound("No personalized set found for this assessment");
            }

            return Ok(personalizedSet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting personalized set for assessment {AssessmentId}", assessmentId);
            return StatusCode(500, "Internal server error");
        }
    }


} 