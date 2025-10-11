using Domain.Customers;
using Domain.Products;

namespace Domain.Orders;

public class Order
{
    private readonly HashSet<LineItem> lineItems = new();

    private Order(Guid id, Guid customerId)
    {
        Id = id;
        CustomerId = customerId;
    }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public static Order Create(Customer customer)
    {
        var order = new Order(id: Guid.NewGuid(), customerId: customer.Id);
        return order;
    }

    public void Add(Product product)
    {
        var lineItem = new LineItem(
            id: Guid.NewGuid(),
            orderId: Id,
            productId: product.Id,
            price: product.Price);

        lineItems.Add(lineItem);
    }
}
