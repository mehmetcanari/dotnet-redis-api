using Microsoft.AspNetCore.Mvc;
using Redis.Application.Abstract.Service;
using Redis.Application.DTO.Request;

namespace dotnet_redis_demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;
    private readonly ILogger<ScoreController> _logger;

    public ScoreController(IScoreService scoreService, ILogger<ScoreController> logger)
    {
        _scoreService = scoreService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllScores()
    {
        try
        {
            var scores = await _scoreService.GetAllScoresAsync();

            if (scores == null || !scores.Any())
            {
                return NotFound("No scores found.");
            }

            return Ok(scores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all scores");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScoreById([FromRoute] int id)
    {
        try
        {
            var score = await _scoreService.GetScoreByIdAsync(id);

            if (score == null)
            {
                return NotFound("Score not found.");
            }

            return Ok(score);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving score with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateScore([FromBody] ScoreCreateRequestDto createRequestDto)
    {
        try
        {
            await _scoreService.CreateScoreAsync(createRequestDto);
            return Ok("Score created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating score");
            return StatusCode(500, "Internal server error");
        }
    }
}