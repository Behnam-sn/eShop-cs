namespace Application.Products.CreateProduct;

public sealed record ProductCreatedEvent(
    long Id,
    string Name,
    decimal Price);
