namespace PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

public sealed record StoreIdentifier
{
    public StoreIdentifier(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("StoreIdentifier cannot be an empty GUID.", nameof(value));

        Value = value;
    }
    
    public Guid Value { get; }
    
    public override string ToString() => Value.ToString();
}