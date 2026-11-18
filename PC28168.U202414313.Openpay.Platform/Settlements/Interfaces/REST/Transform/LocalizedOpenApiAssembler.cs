using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Resources;

namespace PC28168.U202414313.Openpay.Platform.Settlements.Interfaces.REST.Transform;

public static class LocalizedOpenApiAssembler
{
    private static readonly Dictionary<string, (string Summary, string Description,
        IReadOnlyDictionary<string, string> Responses)> OperationResources =
        new()
        {
            ["CreateDisbursementOrder"] = (
                "Create a disbursement order",
                "Registers a new disbursement order with unique orderTrackingCode validation and returns the persisted order resource.",
                new Dictionary<string, string>
                {
                    ["201"] = "The disbursement order was created successfully.",
                    ["400"] = "The request payload contains missing or invalid values.",
                    ["409"] = "A disbursement order with the same orderTrackingCode already exists.",
                    ["500"] = "An unexpected server error occurred while creating the disbursement order."
                })
        };
    
    public static Task TransformOperationAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor action ||
            !OperationResources.TryGetValue(action.ActionName, out var resources))
            return Task.CompletedTask;

        operation.OperationId = action.ActionName;
        operation.Summary = resources.Summary;
        operation.Description = resources.Description;

        if (operation.Responses is null) return Task.CompletedTask;

        foreach (var (statusCode, description) in resources.Responses)
        {
            if (operation.Responses.TryGetValue(statusCode, out var response) &&
                response is OpenApiResponse openApiResponse)
                openApiResponse.Description = description;
        }

        return Task.CompletedTask;
    }
    
    public static Task TransformSchemaAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        if (type == typeof(CreateDisbursementOrderResource))
            schema.Description = "Request payload for registering a new disbursement order.";
        else if (type == typeof(DisbursementOrderResource))
            schema.Description = "Flattened representation of a registered disbursement order.";

        var propertyDescription = context.JsonPropertyInfo?.Name switch
        {
            "id" => "Server-generated unique identifier of the disbursement order.",
            "orderTrackingCode" => "Unique external Openpay GUID tracking reference.",
            "affiliateStoreId" => "Unique GUID identifier of the affiliated store.",
            "scheduledFrom" => "Scheduled execution start date (YYYY-MM-DD).",
            "scheduledTo" => "Scheduled execution end date (YYYY-MM-DD).",
            "transferCapAmount" => "Maximum monetary transfer amount (greater than or equal to 0).",
            "transferCapCurrencyCode" => "ISO currency code for the transfer cap.",
            "status" => "Disbursement order status (Queued, Processed, Cancelled, or Failed).",
            "user" => "Operator user identifier who registered the order (max 20 characters).",
            "remarks" => "Optional notes or remarks (max 85 characters).",
            "updatedAt" => "UTC timestamp of the last update to the disbursement order.",
            _ => null
        };

        if (propertyDescription is not null)
            schema.Description = propertyDescription;

        return Task.CompletedTask;
    }
}