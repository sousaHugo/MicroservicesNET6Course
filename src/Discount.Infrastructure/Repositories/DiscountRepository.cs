using Dapper;
using Discount.Domain.Entities;
using Discount.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _config;
    public DiscountRepository(IConfiguration Config)
    {
        _config = Config ?? throw new ArgumentNullException(nameof(Config));
    }
    public async Task<bool> Add(Coupon Coupon)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affectedRows =
            await connection.ExecuteAsync
            ("INSERT INTO \"Coupon\" (\"ProductName\", \"Description\", \"Amount\") VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = Coupon.ProductName, Description = Coupon.Description, Amount = Coupon.Amount });

        return affectedRows > 0;
    }

    public async Task<bool> Delete(string ProductName)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affectedRows =
            await connection.ExecuteAsync
            ("DELETE FROM \"Coupon\" WHERE \"ProductName\" = @ProductName",
                new { ProductName });

        return affectedRows > 0;
    }

    public async Task<Coupon> Get(string ProductName)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            ($"SELECT * FROM \"Coupon\" WHERE \"ProductName\" = @ProductName", new { ProductName });

        return coupon;
    }

    public async Task<Coupon> Get(int Id)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            ("SELECT * FROM \"Coupon\" WHERE \"Id\" = @Id", new { Id });

        return coupon;
    }

    public async Task<bool> Update(Coupon Coupon)
    {
        using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affectedRows =
            await connection.ExecuteAsync
            ("UPDATE \"Coupon\" SET \"ProductName\" = @ProductName, \"Description\" = @Description, \"Amount\" = @Amount WHERE \"Id\" = @Id",
                new { Coupon.ProductName, Coupon.Description, Coupon.Amount, Coupon.Id });

        return affectedRows > 0;
    }
}
