namespace PC28168.U202414313.Openpay.Platform.Shared.Application.Patterns;

public abstract record Result<TValue, TError>
{
    public sealed record Success(TValue Value) : Result<TValue, TError>;
    public sealed record Failure(TError Error) : Result<TValue, TError>;
}