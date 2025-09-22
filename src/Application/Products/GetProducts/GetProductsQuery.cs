using Application.Abstractions.Messaging;

namespace Application.Products.GetProducts;

public sealed record GetProductsQuery(int Page, int PageSize) : IQuery<List<ProductResponse>>;
