using Microsoft.Extensions.Logging;
using Redis.Application.Abstract.Service;
using Redis.Application.DTO.Request;
using Redis.Application.DTO.Response;
using Redis.Domain.Entities;
using Redis.Infrastructure.Abstract;
using Redis.Infrastructure.Abstract.Repository;

namespace Redis.Application.Services;

public class ScoreService : IScoreService
{
    private readonly ILogger<ScoreService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Score> _scoreRepository;
    private readonly ICacheService _cacheService;
    private const string GetAllScoresCacheKey = "GetAllScores";
    private const string GetScoreByIdCacheKey = "GetScoreById";
    
    public ScoreService(ILogger<ScoreService> logger, IUnitOfWork unitOfWork, IGenericRepository<Score> scoreRepository, ICacheService cacheService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _scoreRepository = scoreRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<ScoreResponseDto>> GetAllScoresAsync()
    {
        try
        {
            var cachedScores = await _cacheService.GetCacheAsync<IEnumerable<ScoreResponseDto>>(GetAllScoresCacheKey);
            if (cachedScores != null)
            {
                Console.WriteLine("Cache hit for GetAllScores");
                return cachedScores;
            }
            
            var scores = await _scoreRepository.GetAllEntitiesAsync();
            var scoreResponses = scores.Select(score => new ScoreResponseDto
            {
                Player = score.Player,
                Score = score.Value,
            });
            
            await _cacheService.SetCacheAsync(GetAllScoresCacheKey, scoreResponses, TimeSpan.FromMinutes(5));

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
            var cachedScore = await _cacheService.GetCacheAsync<ScoreResponseDto>($"{GetScoreByIdCacheKey}_{id}");
            if (cachedScore != null)
            {
                Console.WriteLine($"Cache hit for GetScoreById with ID {id}");
                return cachedScore;
            }
            
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
            
            await _cacheService.SetCacheAsync($"{GetScoreByIdCacheKey}_{id}", scoreResponse, TimeSpan.FromMinutes(5));
            
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
            await _cacheService.SetCacheAsync(GetAllScoresCacheKey, score, TimeSpan.FromMinutes(5));
            await _cacheService.InvalidateCacheAsync([$"{GetScoreByIdCacheKey}_{score.Id}",GetAllScoresCacheKey]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating a score.");
            throw;
        }
    }
}