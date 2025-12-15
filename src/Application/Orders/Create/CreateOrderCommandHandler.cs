using Application.Data;
using Domain.Customers;
using Domain.Orders;
using MediatR;
using Rebus.Bus;

namespace Application.Orders.Create;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IBus _bus;

    private readonly IRepository<Customer> _customerRepository;

    private readonly IRepository<Order> _orderRepository;

    public CreateOrderCommandHandler(IBus bus, IRepository<Customer> customerRepository, IRepository<Order> orderRepository)
    {
        _bus = bus;
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

        await _bus.Send(new OrderCreatedEvent(order.Id.Value));
    }
}
