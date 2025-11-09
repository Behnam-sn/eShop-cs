using MediatR;

namespace Application.Orders.Create;

public record GetOrderQuery(Guid OrderId) : IRequest<OrderResponse>;
