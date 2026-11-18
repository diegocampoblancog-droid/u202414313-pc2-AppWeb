using PC28168.U202414313.Openpay.Platform.Shared.Domain.Model;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;

public partial class DisbursementOrder : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}