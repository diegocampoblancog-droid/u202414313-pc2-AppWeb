using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Resources;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Transform;

public static class DisbursementOrderResourceFromEntityAssembler
{

    public static DisbursementOrderResource ToResourceFromEntity(DisbursementOrder entity) =>
        new(
            entity.Id,
            entity.OrderTrackingCode.Value.ToString(),
            entity.AffiliateStoreId.Value.ToString(),
            entity.ExecutionSchedule.ScheduledFrom.ToString("yyyy-MM-dd"),
            entity.ExecutionSchedule.ScheduledTo.ToString("yyyy-MM-dd"),
            entity.TransferCap.Amount,
            entity.TransferCap.CurrencyCode,
            entity.Status.ToString(),
            entity.User,
            entity.Remarks ?? string.Empty,
            entity.UpdatedAt);
}