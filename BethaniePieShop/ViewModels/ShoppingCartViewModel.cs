using BethaniePieShop.Models;

namespace BethaniePieShop.ViewModels;

public class ShoppingCartViewModel
{
    public ShoppingCartViewModel(
        IShoppingCart shoppingCart, decimal shoppingCartTotal)
    {
        this.ShoppingCart = shoppingCart;
        this.ShoppingCartTotal = shoppingCartTotal;
    }

    public IShoppingCart ShoppingCart { get; private set; }
    public decimal ShoppingCartTotal { get; private set; }
}
