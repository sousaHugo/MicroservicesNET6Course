using Discount.Domain.Entities;
using Discount.Domain.Interfaces;

namespace Discount.Application.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _repository;
    public DiscountService(IDiscountRepository Repository)
    {
        _repository = Repository ?? throw new ArgumentNullException(nameof(Repository));
    }
    public async Task<bool> CreateCouponAsync(Coupon Coupon)
    {
        var coupon = await _repository.Get(Coupon.ProductName);

        if (coupon is not null)
            throw new Exception($"Coupon for the Product {Coupon.ProductName} already exists!");

        return await _repository.Add(Coupon);
    }

    public async Task<bool> DeleteCouponAsync(string ProductName)
    {
        var coupon = await _repository.Get(ProductName);

        if (coupon is not null)
            throw new Exception($"Coupon for the Product {ProductName} doesn't exists!");

        return await _repository.Delete(ProductName);
    }

    public async Task<Coupon> GetCouponByProductNameAsync(string ProductName)
    {
        var coupon = await _repository.Get(ProductName);

        if (coupon is null)
            return new Coupon() { Amount = 0, Description = "No Coupon Available", ProductName = "No Coupon Available" };

        return coupon;
    }

    public async Task<bool> UpdateCouponAsync(Coupon Coupon)
    {
        var coupon = await _repository.Get(Coupon.Id);

        if (coupon is not null)
            throw new Exception($"Coupon {Coupon.Id} doesn't exists!");

        coupon.ProductName = Coupon.ProductName;
        coupon.Description = Coupon.Description;
        coupon.Amount = Coupon.Amount;

        return await _repository.Update(coupon);
    }
}
