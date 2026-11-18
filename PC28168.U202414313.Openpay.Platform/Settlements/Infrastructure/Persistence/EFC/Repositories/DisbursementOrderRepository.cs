using Microsoft.EntityFrameworkCore;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U202414313.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using PC28168.U202414313.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Infrastructure.Persistence.EFC.Repositories;

public class DisbursementOrderRepository(AppDbContext context)
    : BaseRepository<DisbursementOrder>(context), IDisbursementOrderRepository
{
    public async Task<bool> ExistsByOrderTrackingCodeAsync(OpenpayReference orderTrackingCode,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<DisbursementOrder>()
            .AnyAsync(d => d.OrderTrackingCode == orderTrackingCode, cancellationToken);
    }
}