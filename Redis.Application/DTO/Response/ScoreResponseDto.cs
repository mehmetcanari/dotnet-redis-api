using Redis.Domain.Entities;

namespace Redis.Application.DTO.Response;

public record ScoreResponseDto
{
    public required Player Player { get; set; }
    public required int Score { get; set; }
}