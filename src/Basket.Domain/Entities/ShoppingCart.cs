namespace Basket.Domain.Entities;

public class ShoppingCart
{
    public ShoppingCart() { }
    public ShoppingCart(string Username)
    {
        this.Username = Username;
    }
    public string Username { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (var item in Items)
                totalPrice += item.Price * item.Quantity;

            return totalPrice;
        }
    }
}
