using Discount.Domain.Entities;

namespace Discount.Application.Services;

public interface IDiscountService
{
    Task<Coupon> GetCouponByProductNameAsync(string ProductName);
    Task<bool> CreateCouponAsync(Coupon Coupon);
    Task<bool> UpdateCouponAsync(Coupon Coupon);
    Task<bool> DeleteCouponAsync(string ProductName);
}
