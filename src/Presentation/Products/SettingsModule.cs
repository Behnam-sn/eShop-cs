using Carter;
using Domain.Outbox;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Products;

public sealed class SettingsModule : CarterModule
{
    public SettingsModule()
        : base("/settings")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/outbox", (OutboxSettings settings) =>
        {
            return Results.Ok(settings);
        });
    }
}
