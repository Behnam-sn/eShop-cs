namespace Domain.Orders;

public record OrderSummary(Guid OrderId, Guid CustomerId, decimal TotalPrice);
