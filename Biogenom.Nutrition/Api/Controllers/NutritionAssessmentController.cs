using Biogenom.Nutrition.Application.DTOs;
using Biogenom.Nutrition.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Biogenom.Nutrition.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class NutritionAssessmentController : ControllerBase
{
    private readonly INutritionAssessmentService _assessmentService;

    public NutritionAssessmentController(INutritionAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(NutritionAssessmentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAssessment([FromBody] CreateAssessmentRequest request)
    {
        var result = await _assessmentService.CreateAssessmentAsync(request);
        
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Errors);
    }



    [HttpGet("report/{assessmentId}")]
    [ProducesResponseType(typeof(AssessmentReportDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAssessmentReportById(int assessmentId)
    {
        var result = await _assessmentService.GetAssessmentReportByIdAsync(assessmentId);
        
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        if (result.Errors.Any(e => e.Message.Contains("not found")))
        {
            return NotFound(result.Errors);
        }
        
        return BadRequest(result.Errors);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NutritionAssessmentDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllAssessments()
    {
        var result = await _assessmentService.GetAllAssessmentsAsync();
        
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Errors);
    }

    [HttpDelete("{assessmentId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAssessment(int assessmentId)
    {
        var result = await _assessmentService.DeleteAssessmentAsync(assessmentId);
        
        if (result.IsSuccess)
        {
            return Ok();
        }
        
        if (result.Errors.Any(e => e.Message.Contains("not found")))
        {
            return NotFound(result.Errors);
        }
        
        return BadRequest(result.Errors);
    }
} 