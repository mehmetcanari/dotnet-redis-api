using Microsoft.AspNetCore.Mvc;
using Redis.Application.Abstract.Service;
using Redis.Application.DTO.Request;

namespace dotnet_redis_demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById([FromRoute] int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        return Ok( new { message = "Player retrieved successfully", data = player });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();
        return Ok(new { message = "Players retrieved successfully", data = players });
    }

    
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreateRequestDto createRequestDto)
    {
        await _playerService.CreatePlayerAsync(createRequestDto);
        return Ok ("Player created successfully");
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer([FromRoute] int id, [FromBody] PlayerUpdateRequestDto updateRequestDto)
    {
        await _playerService.UpdatePlayerAsync(id, updateRequestDto);
        return Ok("Player updated successfully");
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer([FromRoute] int id)
    {
        await _playerService.DeletePlayerAsync(id);
        return Ok("Player deleted successfully");
    }
}