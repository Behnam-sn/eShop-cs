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

    public static Order Create(Customer customer)
    {
        var order = new Order(id: new OrderId(Guid.NewGuid()), customerId: customer.Id);
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
