namespace Redis.Application.Abstract;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
}