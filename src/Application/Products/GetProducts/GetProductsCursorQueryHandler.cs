using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;
using Marten;

namespace Application.Products.GetProducts;

internal sealed class GetProductsCursorQueryHandler
    : IQueryHandler<GetProductsCursorQuery, CursorResponse<List<ProductResponse>>>
{
    private readonly IQuerySession _session;

    public GetProductsCursorQueryHandler(IQuerySession session)
    {
        _session = session;
    }

    public async Task<Result<CursorResponse<List<ProductResponse>>>> Handle(GetProductsCursorQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<ProductResponse> products = await _session
            .Query<Product>()
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Price,
                p.Tags))
            .Where(p => p.Id >= request.Cursor)
            .Take(request.PageSize + 1)
            .OrderBy(p => p.Id)
            .ToListAsync(cancellationToken);

        var cursor = products[^1].Id;

        var productResponses = products.Take(request.PageSize).ToList();

        return new CursorResponse<List<ProductResponse>>(cursor, productResponses);
    }
}
