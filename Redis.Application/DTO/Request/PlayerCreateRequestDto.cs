namespace Redis.Application.DTO.Request;

public record PlayerCreateRequestDto
{
    public required string Nickname { get; set; }
    public required string Country { get; set; }
}