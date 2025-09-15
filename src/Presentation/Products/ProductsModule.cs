using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.GetProducts;
using Application.Products.UpdateProduct;
using Carter;
using Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Products;

public class ProductsModule : CarterModule
{
    public ProductsModule()
        : base("/products")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            Result<List<ProductResponse>> result = await sender.Send(new GetProductsQuery());

            return Results.Ok(result.Value);
        });

        app.MapPost("/", async (CreateProductRequest request, ISender sender) =>
        {
            CreateProductCommand command = request.Adapt<CreateProductCommand>();

            await sender.Send(command);

            return Results.Ok();
        });

        app.MapPut("/products/{productId}", async (long productId, [FromBody] UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>() with
            {
                Id = productId
            };

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.NoContent();
        });

        app.MapDelete("/products/{productId}", async (long productId, ISender sender) =>
        {
            var command = new DeleteProductCommand(productId);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.NoContent();
        });
    }
}
