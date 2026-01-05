using MediatR;

namespace Application.Products.UpdateProduct;

public sealed record ProductUpdatedEvent(
    Guid Id,
    string Name,
    decimal Price) : INotification;
