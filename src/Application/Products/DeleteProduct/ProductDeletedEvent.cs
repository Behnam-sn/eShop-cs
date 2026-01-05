using MediatR;

namespace Application.Products.DeleteProduct;

public sealed record ProductDeletedEvent(Guid Id) : INotification;
