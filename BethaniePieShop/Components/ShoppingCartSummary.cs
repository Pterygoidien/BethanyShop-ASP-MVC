
using BethaniePieShop.Models;
using BethaniePieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethaniePieShop.Components;

public class ShoppingCartSummary : ViewComponent
{
    private readonly IShoppingCart _shoppingCart;

    public ShoppingCartSummary(IShoppingCart shoppingCart)
    {
        this._shoppingCart = shoppingCart;
    }

    public IViewComponentResult Invoke()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        _shoppingCart.ShoppingCartItems = items;

        var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());
        return View(shoppingCartViewModel);
    }
}
