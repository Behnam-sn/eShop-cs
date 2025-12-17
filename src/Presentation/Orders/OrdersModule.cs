using Application.Orders.GetOrderSummary;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Orders;

public class OrdersModule : CarterModule
{
    public OrdersModule()
        : base("/orders")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}/summary", async (Guid id, ISender sender) =>
        {
            var command = new GetOrderSummaryQuery(id);

            var result = await sender.Send(command);

            return Results.Ok(result.Value);
        });
    }
}
