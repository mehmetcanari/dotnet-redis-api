using Microsoft.AspNetCore.Mvc;
using Redis.Application.Abstract.Service;
using Redis.Application.DTO.Request;

namespace dotnet_redis_demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;

    public ScoreController(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllScores()
    {
        var scores = await _scoreService.GetAllScoresAsync();
        return Ok( new { message = "Scores retrieved successfully", data = scores });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScoreById([FromRoute] int id)
    {
        var score = await _scoreService.GetScoreByIdAsync(id);
        return Ok( new { message = "Score retrieved successfully", data = score });
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateScore([FromBody] ScoreCreateRequestDto createRequestDto)
    {
        await _scoreService.CreateScoreAsync(createRequestDto);
        return Ok("Score created successfully");
    }
}