namespace PC28168.U202414313.Openpay.Platform.Shared.Domain.Model;

public interface IAuditableEntity
{

    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}