using Microsoft.EntityFrameworkCore;
using PC28168.U202414313.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U202414313.Openpay.Platform.Settlements.Application.Services;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Commands;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Repositories;
using PC28168.U202414313.Openpay.Platform.Shared.Application.Patterns;
using PC28168.U202414313.Openpay.Platform.Shared.Domain.Repositories;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Application.Internal.CommandServices;

public class DisbursementOrderCommandService(
    IDisbursementOrderRepository disbursementOrderRepository,
    IUnitOfWork unitOfWork,
    ILogger<DisbursementOrderCommandService> logger)
    : IDisbursementOrderCommandService
{
    public async Task<Result<DisbursementOrder, CreateDisbursementOrderError>> Handle(
        CreateDisbursementOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var exists = await disbursementOrderRepository.ExistsByOrderTrackingCodeAsync(
            command.OrderTrackingCode, cancellationToken);

        if (exists)
        {
            logger.LogWarning(
                "Duplicate disbursement order rejected for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.DuplicateOrderTrackingCode);
        }

        try
        {
            var disbursementOrder = new DisbursementOrder(command);
            await disbursementOrderRepository.AddAsync(disbursementOrder, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Success(disbursementOrder);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex,
                "Invalid domain argument while creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            throw;
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex,
                "Database unique constraint violation for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.DuplicateOrderTrackingCode);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error creating disbursement order for OrderTrackingCode {OrderTrackingCode}",
                command.OrderTrackingCode);
            return new Result<DisbursementOrder, CreateDisbursementOrderError>.Failure(
                CreateDisbursementOrderError.UnexpectedError);
        }
    }

    private static bool IsDuplicateKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "MySqlException", StringComparison.Ordinal)) continue;
            var numberProperty = current.GetType().GetProperty("Number");
            if (numberProperty?.PropertyType == typeof(int) &&
                numberProperty.GetValue(current) is int errorCode &&
                errorCode == 1062)
                return true;
        }

        return false;
    }
}