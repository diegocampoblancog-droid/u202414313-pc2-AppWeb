using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U202414313.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Transform;


public static class CreateDisbursementOrderCommandFromResourceAssembler
{
    
    public static CreateDisbursementOrderCommand ToCommandFromResource(CreateDisbursementOrderResource resource)
    {
        if (!Guid.TryParse(resource.OrderTrackingCode, out var trackingGuid))
            throw new ArgumentException("OrderTrackingCode must be a valid GUID.", nameof(resource));

        if (!Guid.TryParse(resource.AffiliateStoreId, out var storeGuid))
            throw new ArgumentException("AffiliateStoreId must be a valid GUID.", nameof(resource));

        var status = ParseStatus(resource.Status);

        return new CreateDisbursementOrderCommand(
            new OpenpayReference(trackingGuid),
            new StoreIdentifier(storeGuid),
            new DateInterval(resource.ScheduledFrom, resource.ScheduledTo),
            new FinancialAmount(resource.TransferCapAmount, resource.TransferCapCurrencyCode),
            status,
            resource.User,
            resource.Remarks);
    }

    private static DisbursementStatus ParseStatus(string? rawStatus)
    {
        if (string.IsNullOrWhiteSpace(rawStatus))
            return DisbursementStatus.Queued;

        var trimmed = rawStatus.Trim();
        if (int.TryParse(trimmed, out _) ||
            !Enum.TryParse<DisbursementStatus>(trimmed, ignoreCase: true, out var parsed) ||
            !Enum.IsDefined(parsed))
        {
            throw new ArgumentException(
                "Status must be one of the allowed values: Queued, Processed, Cancelled, or Failed.",
                nameof(rawStatus));
        }

        return parsed;
    }
}