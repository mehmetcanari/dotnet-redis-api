namespace Redis.Domain.Entities;

public class Score
{
    public Guid Id { get; init; }
    public required string PlayerId { get; init; }
    public required int Value { get; init; }
    public required DateTime CreatedAt { get; init; }
    
    #region Navigation Properties
    public Player Player { get; set; } = null!;
    #endregion
    
    public Score(string playerId, int value)
    {
        PlayerId = playerId;
        Value = value;
        CreatedAt = DateTime.UtcNow;
    }
}