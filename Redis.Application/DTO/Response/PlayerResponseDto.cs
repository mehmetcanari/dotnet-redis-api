namespace Redis.Application.DTO.Response;

public record PlayerResponseDto
{
    public required string Nickname { get; set; }
    public required string Country { get; set; }
}