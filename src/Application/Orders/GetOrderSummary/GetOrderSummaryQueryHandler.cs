using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetOrderSummary;

public class GetOrderSummaryQueryHandler : IQueryHandler<GetOrderSummaryQuery, OrderSummary>
{
    private readonly IRepository<OrderSummary> _orderSummaryRepository;

    public GetOrderSummaryQueryHandler(IRepository<OrderSummary> orderSummaryRepository)
    {
        _orderSummaryRepository = orderSummaryRepository;
    }

    public async Task<Result<OrderSummary>> Handle(GetOrderSummaryQuery request, CancellationToken cancellationToken)
    {
        var orderSummary = await _orderSummaryRepository.GetByIdAsync(request.OrderId);

        if (orderSummary is null)
        {
            return Result.Failure<OrderSummary>(Error.NullValue);
        }

        return orderSummary;
    }
}
