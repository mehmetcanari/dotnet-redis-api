using Redis.Application.DTO.Request;
using Redis.Application.DTO.Response;

namespace Redis.Application.Abstract.Service;

public interface IPlayerService
{
    Task <IEnumerable<PlayerResponseDto>> GetAllPlayersAsync();
    Task<PlayerResponseDto> GetPlayerByIdAsync(int id);
    Task CreatePlayerAsync(PlayerCreateRequestDto createRequestDto);
    Task UpdatePlayerAsync(int id, PlayerUpdateRequestDto updateRequestDto);
    Task DeletePlayerAsync(int id);
}