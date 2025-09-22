using Application.Abstractions.Messaging;

namespace Application.Products.GetProducts;

public sealed record GetProductsCursorQuery(int Cursor, int PageSize) : IQuery<List<ProductResponse>>;
