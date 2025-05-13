using Microsoft.AspNetCore.Mvc;
using Redis.Application.Abstract.Service;
using Redis.Application.DTO.Request;

namespace dotnet_redis_demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly ILogger<PlayerController> _logger;
    private readonly IPlayerService _playerService;

    public PlayerController(ILogger<PlayerController> logger, IPlayerService playerService)
    {
        _logger = logger;
        _playerService = playerService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById([FromRoute] int id)
    {
        try
        {
            var player = await _playerService.GetPlayerByIdAsync(id);

            if (player == null)
            {
                return NotFound("Player not found.");
            }

            return Ok(player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving player with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPlayers()
    {
        try
        {
            var players = await _playerService.GetAllPlayersAsync();

            if (players == null || !players.Any())
            {
                return NotFound("No players found.");
            }

            return Ok(players);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all players");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreateRequestDto createRequestDto)
    {
        try
        {
            await _playerService.CreatePlayerAsync(createRequestDto);
            return Ok ("Player created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating player");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer([FromRoute] int id, [FromBody] PlayerUpdateRequestDto updateRequestDto)
    {
        try
        {
            await _playerService.UpdatePlayerAsync(id, updateRequestDto);
            return Ok("Player updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating player with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer([FromRoute] int id)
    {
        try
        {
            await _playerService.DeletePlayerAsync(id);
            return Ok("Player deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting player with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}