using Domain.Customers;

namespace Domain.Orders;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(CustomerId id);
}
