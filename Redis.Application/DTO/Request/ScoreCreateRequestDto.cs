namespace Redis.Application.DTO.Request;

public record ScoreCreateRequestDto
{
    public required int PlayerId { get; set; }
    public required int Score { get; set; }
}