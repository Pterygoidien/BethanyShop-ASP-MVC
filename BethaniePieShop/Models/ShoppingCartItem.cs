namespace BethaniePieShop.Models;

public class ShoppingCartItem
{
    public int ShoppingCartItemid { get; set; }
    public Pie Pie { get; set; } = default!; // default! is a null-forgiving operator, meaning that the property will never be null
    public int Amount { get; set; }
    public string? ShoppingCartId { get; set; }


}
