using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Marten;
using MassTransit;

namespace Application.Products.CreateProduct;

internal sealed class CreateProductCommandHandler
    : ICommandHandler<CreateProductCommand>
{
    private readonly IDocumentSession _session;

    private readonly IEventBus _eventBus;

    public CreateProductCommandHandler(IDocumentSession session, IEventBus eventBus)
    {
        _session = session;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Tags = request.Tags
        };

        _session.Store(product);

        await _session.SaveChangesAsync(cancellationToken);

        await _eventBus.PublishAsync(
            new ProductCreatedEvent(product.Id, product.Name, product.Price),
            cancellationToken);

        return Result.Success();
    }
}
