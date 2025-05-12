namespace Redis.Domain.Entities;

public class Score
{
    public int Id { get; init; }
    public required int Value { get; init; }
    public required DateTime CreatedAt { get; init; }
    
    #region Navigation Properties
    public required int PlayerId { get; init; }
    public Player Player { get; init; } = null!;
    #endregion
}