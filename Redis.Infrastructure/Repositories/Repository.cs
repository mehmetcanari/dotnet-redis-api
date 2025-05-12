using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Redis.Application.Abstract;
using Redis.Application.Abstract.Repository;
using Redis.Persistence.Context;

namespace Redis.Infrastructure.Repositories;

public class Repository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDatabaseContext _context;
    private readonly ILogger <Repository<TEntity>> _logger;

    public Repository(ApplicationDatabaseContext context, ILogger<Repository<TEntity>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync()
    {
        try
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            var entities = await query
                .AsNoTracking()
                .ToListAsync();
            
            return entities;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving entities.");
            throw;
        }
    }
    
    public async Task<TEntity> GetEntityByIdAsync(int id)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving an entity.");
            throw;
        }
    }

    public async Task AddAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while adding an entity.");
            throw;
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while updating an entity.");
            throw;
        }

    }

    public async Task DeleteAsync(TEntity entity)
    {
        try
        {
            var obj = await _context.Set<TEntity>().FindAsync(entity);
            if (obj != null)
            {
                _context.Set<TEntity>().Remove(obj);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while deleting an entity.");
            throw;
        }
    }
}