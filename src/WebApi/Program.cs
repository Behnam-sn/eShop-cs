using Application;
using Application.Abstractions.Caching;
using Application.Abstractions.EventBus;
using Application.Behaviors;
using Application.Orders.Create;
using Application.Products.CreateProduct;
using Carter;
using Domain.Outbox;
using Domain.Products;
using Infrastructure.Caching;
using Infrastructure.MessageBroker;
using Marten;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Persistence;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<OutboxSettings>()
    .BindConfiguration("Outbox")
    .ValidateDataAnnotations()
    .Validate(settings =>
    {
        if (settings.IntervalInSeconds == 0)
        {
            return false;
        }

        return true;
    })
    .ValidateOnStart();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<OutboxSettings>>().Value);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<Product>().SoftDeleted();
});

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<ProductCreatedEventConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        var settings = context.GetRequiredService<MessageBrokerSettings>();

        configurator.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
    });
});

builder.Services.AddScoped<IEventBus, EventBus>();

builder.Services.AddMediatR(ApplicationAssembly.Instance);

builder.Services.AddScoped(
    typeof(IPipelineBehavior<,>),
    typeof(LoggingPipelineBehavior<,>));

builder.Services.AddPersistence(builder.Configuration);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddRebus(
    rebus => rebus
        .Routing(r => r.TypeBased().MapAssemblyOf<ApplicationAssemblyReference>("eshop-queue"))
        .Transport(t => t.UseRabbitMq(
            builder.Configuration.GetConnectionString("MessageBroker"),
            "eshop-queue"))
        .Sagas(s => s.StoreInPostgres(
            builder.Configuration.GetConnectionString("Database"),
            "sagas",
            "sagas_indexes")),
    onCreated: async bus =>
    {
        await bus.Subscribe<OrderConfirmationEmailSent>();
        await bus.Subscribe<OrderPaymentRequestSent>();
    });

builder.Services.AutoRegisterHandlersFromAssemblyOf<ApplicationAssemblyReference>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.MapCarter();

app.UseHttpsRedirection();

app.Run();
