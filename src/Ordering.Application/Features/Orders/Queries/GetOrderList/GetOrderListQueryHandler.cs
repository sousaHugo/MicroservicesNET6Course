using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, IEnumerable<OrdersVm>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    public GetOrderListQueryHandler(IOrderRepository OrderRepository, IMapper Mapper)
    {
        _orderRepository = OrderRepository ?? throw new ArgumentNullException(nameof(OrderRepository));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
    }

    public async Task<IEnumerable<OrdersVm>> Handle(GetOrderListQuery Request, CancellationToken CancellationToken)
    {
        var orderList = await _orderRepository.GetOrderByUsername(Request.Username);

        return _mapper.Map<List<OrdersVm>>(orderList);
    }
}
