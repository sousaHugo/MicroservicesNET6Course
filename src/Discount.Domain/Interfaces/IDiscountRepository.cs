using Discount.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.Interfaces;
public interface IDiscountRepository
{
    Task<Coupon> Get(string ProductName);
    Task<Coupon> Get(int Id);
    Task<bool> Add(Coupon Coupon);
    Task<bool> Update(Coupon Coupon);
    Task<bool> Delete(string ProductName);
}
