namespace PC28168.U202414313.Openpay.Platform.Shared.Domain.Repositories;

public interface IBaseRepository<in TEntity>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
}