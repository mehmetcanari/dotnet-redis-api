namespace Redis.Domain.Entities;

public class Player
{
    public int Id { get; init; }
    public required string Nickname { get; set; }
    public required string Country { get; set; }
    public required DateTime CreatedAt { get; init; }

    #region Navigation Properties
    public ICollection<Score> Scores { get; set; } = new List<Score>();
    
    #endregion
}