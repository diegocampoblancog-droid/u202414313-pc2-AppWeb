using System.ComponentModel.DataAnnotations;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Resources;

public record CreateDisbursementOrderResource(
    [Required(ErrorMessage = "RequiredOrderTrackingCode")]
    string OrderTrackingCode,
    [Required(ErrorMessage = "RequiredAffiliateStoreId")]
    string AffiliateStoreId,
    [Required(ErrorMessage = "RequiredScheduledFrom")]
    DateOnly ScheduledFrom,
    [Required(ErrorMessage = "RequiredScheduledTo")]
    DateOnly ScheduledTo,
    [Required(ErrorMessage = "RequiredTransferCapAmount")]
    decimal TransferCapAmount,
    [Required(ErrorMessage = "RequiredTransferCapCurrencyCode")]
    string TransferCapCurrencyCode,
    string? Status,
    [Required(ErrorMessage = "RequiredUser")]
    string User,
    string? Remarks);