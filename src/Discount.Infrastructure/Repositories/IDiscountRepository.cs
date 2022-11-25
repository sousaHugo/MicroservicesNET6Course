using Discount.Domain.Entities;

namespace Discount.Infrastructure.Repositories;

public interface IDiscountRepository
{
    Task<Coupon> Get(string ProductName);
    Task<Coupon> Get(int Id);
    Task<bool> Add(Coupon Coupon);
    Task<bool> Update(Coupon Coupon);
    Task<bool> Delete(string ProductName);    
}
