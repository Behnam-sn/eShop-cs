using Application;
using Application.Products.CreateProduct;
using Application.Products.GetProducts;
using Domain.Shared;
using Mapster;
using Marten;
using MediatR;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
});

builder.Services.AddMediatR(ApplicationAssembly.Instance);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/products", async (ISender sender) =>
{
    Result<List<ProductResponse>> result = await sender.Send(new GetProductsQuery());

    return Results.Ok(result.Value);
});

app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
{
    CreateProductCommand command = request.Adapt<CreateProductCommand>();

    await sender.Send(command);

    return Results.Ok();
});

app.UseHttpsRedirection();

app.Run();
