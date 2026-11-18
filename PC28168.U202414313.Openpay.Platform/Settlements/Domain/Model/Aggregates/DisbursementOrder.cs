using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.ValueObjects;
using PC28168.U202414313.Openpay.Platform.Shared.Domain.Model.ValueObjects;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;

public partial class DisbursementOrder
{
    private const int MaxUserLength = 20;
    private const int MaxRemarksLength = 85;
    
    protected DisbursementOrder()
    {
        OrderTrackingCode = null!;
        AffiliateStoreId = null!;
        ExecutionSchedule = null!;
        TransferCap = null!;
        User = string.Empty;
    }

    public DisbursementOrder(CreateDisbursementOrderCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.User))
            throw new ArgumentException("User is required and cannot be empty or whitespace.", nameof(command));

        var trimmedUser = command.User.Trim();
        if (trimmedUser.Length > MaxUserLength)
            throw new ArgumentException($"User cannot exceed {MaxUserLength} characters.", nameof(command));

        string? normalizedRemarks = null;
        if (command.Remarks is not null)
        {
            if (command.Remarks.Length > MaxRemarksLength)
                throw new ArgumentException($"Remarks cannot exceed {MaxRemarksLength} characters.", nameof(command));

            normalizedRemarks = command.Remarks;
        }

        if (!Enum.IsDefined(command.Status))
            throw new ArgumentException("Invalid DisbursementStatus value.", nameof(command));

        OrderTrackingCode = command.OrderTrackingCode;
        AffiliateStoreId = command.AffiliateStoreId;
        ExecutionSchedule = command.ExecutionSchedule;
        TransferCap = command.TransferCap;
        Status = command.Status;
        User = trimmedUser;
        Remarks = normalizedRemarks;
    }

    public int Id { get; private set; }

    public OpenpayReference OrderTrackingCode { get; private set; }

    public StoreIdentifier AffiliateStoreId { get; private set; }

    public DateInterval ExecutionSchedule { get; private set; }
    
    public FinancialAmount TransferCap { get; private set; }

    public DisbursementStatus Status { get; private set; }
    
    public string User { get; private set; }

    public string? Remarks { get; private set; }
}