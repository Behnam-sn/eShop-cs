using MediatR;

namespace Application.Orders.Create;

public record OrderPaymentRequestSent(Guid OrderId);
