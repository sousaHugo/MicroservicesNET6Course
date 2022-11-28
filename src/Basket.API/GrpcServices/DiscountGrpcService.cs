using Discount.Gprc;
using static Discount.Gprc.DiscountService;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountServiceClient _client;

        public DiscountGrpcService(DiscountServiceClient Client)
        {
            _client = Client ?? throw new ArgumentNullException(nameof(Client));
        }

        public async Task<CouponModel> GetDiscountByProductName(string ProductName)
        {
            return await _client.GetDiscountAsync(new GetDiscountRequest() { ProductName = ProductName });
        }
    }
}
