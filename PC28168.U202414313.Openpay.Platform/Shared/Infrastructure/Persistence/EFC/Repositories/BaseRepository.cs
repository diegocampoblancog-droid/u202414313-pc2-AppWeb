using PC28168.U202414313.Openpay.Platform.Shared.Domain.Repositories;
using PC28168.U202414313.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace PC28168.U202414313.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

public class BaseRepository<TEntity>(AppDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context = context;

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }
}