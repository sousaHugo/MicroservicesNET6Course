using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public class GetOrderListQuery : IRequest<IEnumerable<OrdersVm>>
{
    public string Username { get; set; }

    public GetOrderListQuery(string Username)
    {
        this.Username = Username ?? throw new ArgumentNullException(nameof(Username));
    }
}
