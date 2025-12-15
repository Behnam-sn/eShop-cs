using Domain.Orders;
using MediatR;

namespace Application.Orders.Create;

public record OrderCreatedEvent(Guid OrderId);
