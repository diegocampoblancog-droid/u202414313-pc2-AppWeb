using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PC28168.U202414313.Openpay.Platform.Resources;
using PC28168.U202414313.Openpay.Platform.Settlements.Application.Errors;
using PC28168.U202414313.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202414313.Openpay.Platform.Shared.Application.Patterns;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Transform;


public static class ActionResultFromCreateDisbursementOrderResultAssembler
{
    public static ActionResult ToActionResultFromCreateDisbursementOrderResult(
        Result<DisbursementOrder, CreateDisbursementOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<DisbursementOrder, CreateDisbursementOrderError>.Success success =>
                controller.Created(
                    $"/api/v1/disbursement-orders/{success.Value.Id}",
                    DisbursementOrderResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<DisbursementOrder, CreateDisbursementOrderError>.Failure failure =>
                failure.Error switch
                {
                    CreateDisbursementOrderError.DuplicateOrderTrackingCode =>
                        controller.Conflict(localizer["DisbursementOrderTrackingCodeDuplicated"].Value),

                    CreateDisbursementOrderError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingDisbursementOrder"].Value,
                            statusCode: StatusCodes.Status500InternalServerError),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: StatusCodes.Status500InternalServerError)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: StatusCodes.Status500InternalServerError)
        };
}