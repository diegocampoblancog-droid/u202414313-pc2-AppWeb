using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202414313.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Commands;

public record CreateDisbursementOrderCommand(
    OpenpayReference OrderTrackingCode,
    StoreIdentifier AffiliateStoreId,
    DateInterval ExecutionSchedule,
    FinancialAmount TransferCap,
    DisbursementStatus Status,
    string User,
    string? Remarks);