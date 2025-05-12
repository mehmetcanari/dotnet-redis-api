using Microsoft.Extensions.Logging;
using Redis.Application.Abstract.Service;
using Redis.Application.DTO.Request;
using Redis.Application.DTO.Response;
using Redis.Domain.Entities;
using Redis.Infrastructure.Abstract;
using Redis.Infrastructure.Abstract.Repository;

namespace Redis.Application.Services;

public class PlayerService : IPlayerService
{
    private readonly ILogger<PlayerService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Player> _playerRepository;
    private readonly ICacheService _cacheService;
    private const string GetAllPlayersCacheKey = "GetAllPlayers";
    private const string GetPlayerByIdCacheKey = "GetPlayerById";

    public PlayerService(ILogger<PlayerService> logger, IUnitOfWork unitOfWork, IGenericRepository<Player> playerRepository, ICacheService cacheService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<PlayerResponseDto>> GetAllPlayersAsync()
    {
        try
        {
            var cachedPlayers = await _cacheService.GetCacheAsync<IEnumerable<PlayerResponseDto>>(GetAllPlayersCacheKey);
            if (cachedPlayers != null)
            {
                Console.WriteLine("Cache hit for GetAllPlayers");
                return cachedPlayers;
            }
            
            var players = await _playerRepository.GetAllEntitiesAsync();
            var playerResponses = players.Select(player => new PlayerResponseDto
            {
                Nickname = player.Nickname,
                Country = player.Country,
            });
            
            await _cacheService.SetCacheAsync(GetAllPlayersCacheKey, playerResponses, TimeSpan.FromMinutes(5));

            return playerResponses;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving players.");
            throw;
        }
    }

    public async Task<PlayerResponseDto> GetPlayerByIdAsync(int id)
    {
        try
        {
            var cachedPlayer = await _cacheService.GetCacheAsync<PlayerResponseDto>($"{GetPlayerByIdCacheKey}_{id}");
            if (cachedPlayer != null)
            {
                Console.WriteLine($"Cache hit for GetPlayerById with ID {id}");
                return cachedPlayer;
            }
            
            var player = await _playerRepository.GetEntityByIdAsync(id);
            if (player == null)
            {
                throw new KeyNotFoundException($"Player with ID {id} not found.");
            }
            
            var playerResponse = new PlayerResponseDto
            {
                Nickname = player.Nickname,
                Country = player.Country,
            };
            
            await _cacheService.SetCacheAsync($"{GetPlayerByIdCacheKey}_{id}", playerResponse, TimeSpan.FromMinutes(5));

            return playerResponse;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving the player.");
            throw;
        }
    }

    public async Task CreatePlayerAsync(PlayerCreateRequestDto createRequestDto)
    {
        try
        {
            var players = await _playerRepository.GetAllEntitiesAsync();
            if (players.Any(p => p.Nickname.Equals(createRequestDto.Nickname, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Player with nickname {createRequestDto.Nickname} already exists.");
            }
            
            var player = new Player
            {
                Nickname = createRequestDto.Nickname,
                Country = createRequestDto.Country,
                CreatedAt = DateTime.UtcNow,
            };
            
            await _playerRepository.AddAsync(player);
            await _unitOfWork.CommitAsync();
            await _cacheService.SetCacheAsync(GetAllPlayersCacheKey, player, TimeSpan.FromMinutes(5));
            await _cacheService.InvalidateCacheAsync([
                $"{GetPlayerByIdCacheKey}_{player.Id}",
                GetAllPlayersCacheKey
            ]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating the player.");
            throw;
        }
    }

    public async Task UpdatePlayerAsync(int id, PlayerUpdateRequestDto updateRequestDto)
    {
        try
        {
            var player = await _playerRepository.GetEntityByIdAsync(id);
            if (player == null)
            {
                throw new KeyNotFoundException($"Player with ID {id} not found.");
            }

            player.Nickname = updateRequestDto.Nickname;
            player.Country = updateRequestDto.Country;

            await _playerRepository.UpdateAsync(player);
            await _unitOfWork.CommitAsync();
            await _cacheService.SetCacheAsync($"{GetPlayerByIdCacheKey}_{id}", player, TimeSpan.FromMinutes(5));
            await _cacheService.InvalidateCacheAsync([
                $"{GetPlayerByIdCacheKey}_{player.Id}",
                GetAllPlayersCacheKey
            ]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while updating the player.");
            throw;
        }
    }

    public async Task DeletePlayerAsync(int id)
    {
        try
        {
            var player = await _playerRepository.GetEntityByIdAsync(id);
            if (player == null)
            {
                throw new KeyNotFoundException($"Player with ID {id} not found.");
            }

            await _playerRepository.DeleteAsync(player);
            await _unitOfWork.CommitAsync();
            await _cacheService.InvalidateCacheAsync([
                $"{GetPlayerByIdCacheKey}_{id}",
                GetAllPlayersCacheKey
            ]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while deleting the player.");
            throw;
        }
    }
}