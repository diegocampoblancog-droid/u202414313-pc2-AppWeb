namespace PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Resources;

public record DisbursementOrderResource(
    int Id,
    string OrderTrackingCode,
    string AffiliateStoreId,
    string ScheduledFrom,
    string ScheduledTo,
    decimal TransferCapAmount,
    string TransferCapCurrencyCode,
    string Status,
    string User,
    string Remarks,
    DateTimeOffset? UpdatedAt);