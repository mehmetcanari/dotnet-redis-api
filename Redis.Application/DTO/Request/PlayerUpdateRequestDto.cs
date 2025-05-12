namespace Redis.Application.DTO.Request;

public record PlayerUpdateRequestDto
{
    public required string Nickname { get; set; }
    public required string Country { get; set; }
}