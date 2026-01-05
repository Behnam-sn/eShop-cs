using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Marten;
using MediatR;

namespace Application.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IDocumentSession _session;

    private readonly IPublisher _publisher;

    public UpdateProductCommandHandler(IDocumentSession session, IPublisher publisher)
    {
        _session = session;
        _publisher = publisher;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            return Result.Failure(new Error("Product.NotFound", $"Product with id {request.Id} was not found."));
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Tags = request.Tags;

        _session.Update(product);

        await _session.SaveChangesAsync(cancellationToken);

        var productUpdatedEvent = new ProductUpdatedEvent(product.Id.Value, product.Name, product.Price.Amount);
        await _publisher.Publish(productUpdatedEvent, cancellationToken);

        return Result.Success();
    }
}
