using Redis.Domain.Entities;

namespace Redis.Infrastructure.Abstract.Repository;

public interface IScoreRepository
{
    Task <IEnumerable<Score>> GetAllScoresAsync();
    Task<Score> GetScoreByIdAsync(int id);
    Task CreateScoreAsync(Score score);
}