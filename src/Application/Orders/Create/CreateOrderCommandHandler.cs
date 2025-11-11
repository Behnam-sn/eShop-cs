using Application.Data;
using Domain.Customers;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Create;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IPublisher _publisher;

    private readonly IRepository<Customer> _customerRepository;

    private readonly IRepository<Order> _orderRepository;

    public CreateOrderCommandHandler(IPublisher publisher, IRepository<Customer> customerRepository, IRepository<Order> orderRepository)
    {
        _publisher = publisher;
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            return;
        }

        var order = Order.Create(customer.Id);

        _orderRepository.Insert(order);

        await _orderRepository.SaveChangesAsync();

        await _publisher.Publish(new OrderCreatedEvent(order.Id), cancellationToken);
    }
}
