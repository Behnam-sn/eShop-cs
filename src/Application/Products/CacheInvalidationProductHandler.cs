using Application.Abstractions.Caching;
using Application.Products.CreateProduct;
using Application.Products.DeleteProduct;
using Application.Products.UpdateProduct;
using MediatR;

namespace Application.Products;

public class CacheInvalidationProductHandler : INotificationHandler<ProductCreatedEvent>,
    INotificationHandler<ProductUpdatedEvent>, INotificationHandler<ProductDeletedEvent>
{
    private readonly ICacheService _cacheService;

    public CacheInvalidationProductHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken) =>
        await HandleInternal(cancellationToken: cancellationToken);

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken) =>
        await HandleInternal(cancellationToken: cancellationToken);

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken) =>
        await HandleInternal(cancellationToken: cancellationToken);

    private async Task HandleInternal(int? productId = null, CancellationToken cancellationToken = default) =>
        await _cacheService.RemoveAsync("products", cancellationToken);
}
