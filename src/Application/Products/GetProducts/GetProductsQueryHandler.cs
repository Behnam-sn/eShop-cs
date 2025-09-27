using Application.Abstractions.Caching;
using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Marten;

namespace Application.Products.GetProducts;

internal sealed class GetProductsQueryHandler
    : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly ICacheService _cacheService;
    private readonly IQuerySession _session;

    public GetProductsQueryHandler(IQuerySession session, ICacheService cacheService)
    {
        _session = session;
        _cacheService = cacheService;
    }

    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetAsync<List<ProductResponse>>(
            "products",
            async () =>
            {
                var products = await _session
                    .Query<Product>()
                    .Select(p => new ProductResponse(
                        p.Id,
                        p.Name,
                        p.Price,
                        p.Tags))
                    .OrderBy(p => p.Id)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
                return products.ToList();
            },
            cancellationToken);

#pragma warning disable S125

        // var productResponses = await _cacheService.GetAsync<List<ProductResponse>>("products", cancellationToken);
        //
        // if (productResponses is not null)
        // {
        //     return productResponses;
        // }
        //
        // var products = await _session
        //     .Query<Product>()
        //     .Select(p => new ProductResponse(
        //         p.Id,
        //         p.Name,
        //         p.Price,
        //         p.Tags))
        //     .OrderBy(p => p.Id)
        //     .Skip((request.Page - 1) * request.PageSize)
        //     .Take(request.PageSize)
        //     .ToListAsync(cancellationToken);
        //
        // productResponses = products.ToList();
        // await _cacheService.SetAsync("products", productResponses, cancellationToken);
        //
        // return productResponses;
#pragma warning restore S125
    }
}
