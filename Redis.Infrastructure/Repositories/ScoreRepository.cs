using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Redis.Domain.Entities;
using Redis.Infrastructure.Abstract.Repository;
using Redis.Persistence.Context;

namespace Redis.Infrastructure.Repositories;

public class ScoreRepository : IScoreRepository
{
    private readonly ApplicationDatabaseContext _context;
    private readonly ILogger<ScoreRepository> _logger;

    public ScoreRepository(ApplicationDatabaseContext context, ILogger<ScoreRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<IEnumerable<Score>> GetAllScoresAsync()
    {
        try
        {
            IQueryable<Score> query = _context.Scores;
            var scores = await query
                .AsNoTracking()
                .Include(s => s.Player) 
                .ToListAsync();

            return scores;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving scores.");
            throw;
        }
    }

    public async Task<Score> GetScoreByIdAsync(int id)
    {
        try
        {
            IQueryable<Score> query = _context.Scores;
            var score = await query
                .AsNoTracking()
                .Include(s => s.Player) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (score == null)
            {
                throw new KeyNotFoundException($"Score with ID {id} not found.");
            }
            return score;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving a score.");
            throw;
        }
    }

    public async Task CreateScoreAsync(Score score)
    {
        try
        {
            await _context.Scores.AddAsync(score);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating a score.");
            throw;
        }
    }
}