using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PC28168.U202414313.Openpay.Platform.Resources;
using PC28168.U202414313.Openpay.Platform.Settlements.Application.Services;
using PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Resources;
using PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Transform;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Disbursement Orders")]
public class DisbursementOrdersController(
    IDisbursementOrderCommandService disbursementOrderCommandService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<DisbursementOrdersController> logger)
    : ControllerBase
{
  
    [HttpPost]
    [ProducesResponseType(typeof(DisbursementOrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateDisbursementOrder(
        [FromBody] CreateDisbursementOrderResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateDisbursementOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await disbursementOrderCommandService.Handle(command, cancellationToken);
            return ActionResultFromCreateDisbursementOrderResultAssembler.ToActionResultFromCreateDisbursementOrderResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex,
                "Validation failed while creating disbursement order with OrderTrackingCode {OrderTrackingCode}",
                resource.OrderTrackingCode);
            return BadRequest($"{localizer["InvalidDisbursementOrderRequest"].Value} ({ex.Message})");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while creating disbursement order with OrderTrackingCode {OrderTrackingCode}",
                resource.OrderTrackingCode);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingDisbursementOrder"].Value,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}