using Microsoft.Extensions.Logging;
using Redis.Application.Abstract;
using Redis.Application.Abstract.Repository;
using Redis.Application.Abstract.Service;
using Redis.Application.DTO.Request;
using Redis.Application.DTO.Response;
using Redis.Domain.Entities;

namespace Redis.Application.Services;

public class ScoreService : IScoreService
{
    private readonly ILogger<ScoreService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Score> _scoreRepository;
    
    public ScoreService(ILogger<ScoreService> logger, IUnitOfWork unitOfWork, IGenericRepository<Score> scoreRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _scoreRepository = scoreRepository;
    }

    public async Task<IEnumerable<ScoreResponseDto>> GetAllScoresAsync()
    {
        try
        {
            var scores = await _scoreRepository.GetAllEntitiesAsync();
            var scoreResponses = scores.Select(score => new ScoreResponseDto
            {
                Player = score.Player,
                Score = score.Value,
            });

            return scoreResponses;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving scores.");
            throw;
        }
    }

    public async Task<ScoreResponseDto> GetScoreByIdAsync(int id)
    {
        try
        {
            var score = await _scoreRepository.GetEntityByIdAsync(id);
            if (score == null)
            {
                throw new KeyNotFoundException($"Score with ID {id} not found.");
            }

            var scoreResponse = new ScoreResponseDto
            {
                Player = score.Player,
                Score = score.Value,
            };

            return scoreResponse;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving a score.");
            throw;
        }
    }

    public async Task CreateScoreAsync(ScoreCreateRequestDto createRequestDto)
    {
        try
        {
            var score = new Score
            {
                PlayerId = createRequestDto.PlayerId,
                Value = createRequestDto.Score,
                CreatedAt = DateTime.UtcNow,
            };

            await _scoreRepository.AddAsync(score);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating a score.");
            throw;
        }
    }
}