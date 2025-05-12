using Redis.Application.DTO.Request;
using Redis.Application.DTO.Response;

namespace Redis.Application.Abstract.Service;

public interface IScoreService
{
    Task <IEnumerable<ScoreResponseDto>> GetAllScoresAsync();
    Task<ScoreResponseDto> GetScoreByIdAsync(int id);
    Task CreateScoreAsync(ScoreCreateRequestDto createRequestDto);
}