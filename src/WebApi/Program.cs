using Application;
using Application.Products.CreateProduct;
using Application.Products.GetProducts;
using Carter;
using Domain.Shared;
using Mapster;
using Marten;
using MediatR;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

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

app.MapCarter();

app.UseHttpsRedirection();

app.Run();
