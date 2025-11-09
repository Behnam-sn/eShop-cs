namespace Application.Orders.Create;

public record OrderResponse(Guid Id, Guid CustomerId, List<LineItemResponse> LineItems);
