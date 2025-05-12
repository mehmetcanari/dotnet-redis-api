using Redis.Application.DTO.Response;

namespace Redis.Application.DTO.Response;

public record ScoreResponseDto
{
    public required PlayerResponseDto Player { get; set; }
    public required int Score { get; set; }
}