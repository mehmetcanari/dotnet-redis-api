namespace Redis.Domain.Entities;

public class Player
{
    public Guid Id { get; init; }
    public required string Nickname { get; init; }
    public required string Country { get; init; }
    public required DateTime CreatedAt { get; init; }

    #region Navigation Properties
    public ICollection<Score> Scores { get; set; } = new List<Score>();
    
    #endregion
    
    public Player(string nickname, string country)
    {
        Nickname = nickname;
        Country = country;
        CreatedAt = DateTime.UtcNow;
    }
}