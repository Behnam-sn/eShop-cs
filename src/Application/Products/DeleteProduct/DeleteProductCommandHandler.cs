using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Marten;

namespace Application.Products.DeleteProduct;

public sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IDocumentSession _session;

    public DeleteProductCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // 1
        _session.Delete<Product>(request.Id);
        _session.HardDelete<Product>(request.Id);

        // 2
        _session.DeleteWhere<Product>(p => p.Id == request.Id);
        _session.HardDeleteWhere<Product>(p => p.Id == request.Id);

        // 3
        var product = await _session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            return Result.Failure(new Error("Product.NotFound", $"Product with id {request.Id} was not found."));
        }

        _session.Delete(product);
        _session.HardDelete(product);

        await _session.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
