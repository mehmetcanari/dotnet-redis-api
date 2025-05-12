namespace Redis.Infrastructure.Abstract;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
}