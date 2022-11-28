using Discount.Domain.Entities;
using Discount.Infrastructure.Repositories;
using Grpc.Core;

namespace Discount.Gprc.Services
{
    public class BasketService : DiscountService.DiscountServiceBase
    {
        private readonly ILogger<BasketService> _logger;
        private readonly IDiscountRepository _repository;
        public BasketService(ILogger<BasketService> Logger, IDiscountRepository Repository)
        {
            _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
            _repository = Repository ?? throw new ArgumentNullException(nameof(Repository));
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest Request, ServerCallContext Context)
        {
            var discount = await _repository.Get(Request.ProductName);

            if (discount is null)
                return new CouponModel() { Amount = 0, ProductName = "No Product", Description = "No Discount Found.", Id = 0 };

            _logger.LogInformation($"Returning information for the product {Request.ProductName}");

            return new CouponModel()
            {
                Id = discount.Id,
                Amount = discount.Amount,
                Description = discount.Description,
                ProductName = discount.ProductName
            };
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest Request, ServerCallContext Context)
        {
            if (Request.Coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Discount information must be filled in."));

            var coupon = new Coupon()
            {
                Amount = Request.Coupon.Amount,
                Description = Request.Coupon.Description,
                ProductName = Request.Coupon.ProductName
            };

            await _repository.Add(coupon);

            _logger.LogInformation($"Product {Request.Coupon.ProductName} saved with success.");

            var savedDiscount = await _repository.Get(Request.Coupon.ProductName);

            Request.Coupon.Id = savedDiscount.Id;

            return Request.Coupon;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest Request, ServerCallContext Context)
        {
            var discount = await _repository.Get(Request.ProductName);

            if (discount is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Product {Request.ProductName} not found."));

            return new DeleteDiscountResponse() { Success = await _repository.Delete(Request.ProductName) };
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest Request, ServerCallContext Context)
        {
            if (Request.Coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Discount information must be filled in."));

            var discount = await _repository.Get(Request.Coupon.ProductName);

            if (discount is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Product {Request.Coupon.ProductName} not found."));


            discount.Description = Request.Coupon.Description;
            discount.Amount = Request.Coupon.Amount;
            discount.ProductName = Request.Coupon.ProductName;


            if(!await _repository.Update(discount))
                throw new RpcException(new Status(StatusCode.Internal, $"An error occurred trying to update the discount for the Product {Request.Coupon.ProductName}"));

            _logger.LogInformation($"Product {Request.Coupon.ProductName} updated with success.");

            return Request.Coupon;
        }
    }
}