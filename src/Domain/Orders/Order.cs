using Domain.Customers;
using Domain.Products;

namespace Domain.Orders;

public class Order
{
    private readonly HashSet<LineItem> _lineItems = new();

    private Order(OrderId id, CustomerId customerId)
    {
        Id = id;
        CustomerId = customerId;
    }

    public OrderId Id { get; private set; }

    public CustomerId CustomerId { get; private set; }

    public IReadOnlyList<LineItem> LineItems => _lineItems.ToList();

    public static Order Create(CustomerId customerId)
    {
        var order = new Order(id: new OrderId(Guid.NewGuid()), customerId: customerId);
        return order;
    }

    public void AddLineItem(ProductId productId, Money price)
    {
        var lineItem = new LineItem(
            id: new LineItemId(Guid.NewGuid()),
            orderId: Id,
            productId: productId,
            price: price);

        _lineItems.Add(lineItem);
    }
}
