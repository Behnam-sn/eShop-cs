using Domain.Customers;
using Domain.Products;

namespace Domain.Orders;

public class Order
{
    private readonly HashSet<LineItem> lineItems = new();

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

    public void Add(ProductId productId, Money price)
    {
        var lineItem = new LineItem(
            id: new LineItemId(Guid.NewGuid()),
            orderId: Id,
            productId: productId,
            price: price);

        lineItems.Add(lineItem);
    }
}
