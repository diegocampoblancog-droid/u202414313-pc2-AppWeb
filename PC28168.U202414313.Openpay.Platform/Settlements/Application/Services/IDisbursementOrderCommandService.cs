using PC28168.U202414313.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202414313.Openpay.Platform.Shared.Application.Patterns;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Application.Services;


public interface IDisbursementOrderCommandService
{
    Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(
        CreateDisbursementOrderCommand command,
        CancellationToken cancellationToken = default);
}