namespace PC28168.U202414313.Openpay.Platform.Shared.Domain.Model.ValueObjects;

public sealed record FinancialAmount
{
    private FinancialAmount()
    {
        CurrencyCode = string.Empty;
    }
    public FinancialAmount(decimal amount, string currencyCode)
    {
        if (amount < decimal.Zero)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than or equal to zero.");

        ArgumentNullException.ThrowIfNull(currencyCode);
        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ArgumentException("CurrencyCode cannot be empty or whitespace.", nameof(currencyCode));

        Amount = amount;
        CurrencyCode = currencyCode.Trim();
    }

    public decimal Amount { get; }
    
    public string CurrencyCode { get; }
    
    public FinancialAmount MultiplyBy(decimal factor) => new(Amount * factor, CurrencyCode);
}