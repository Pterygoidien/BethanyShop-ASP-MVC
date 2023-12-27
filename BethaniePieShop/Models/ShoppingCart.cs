using Microsoft.EntityFrameworkCore;

namespace BethaniePieShop.Models;

public class ShoppingCart : IShoppingCart
{
    private BethaniePieShopDbContext _bethaniePieShopDbContext;
    public string? ShoppingCartId { get; set; }

    private ShoppingCart(BethaniePieShopDbContext bethaniePieShopDbContext)
    {
        this._bethaniePieShopDbContext = bethaniePieShopDbContext;
    }

    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

    public static ShoppingCart getCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

        BethaniePieShopDbContext context = services.GetService<BethaniePieShopDbContext>() ?? throw new Exception("Error initializing database context");

        string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

        session?.SetString("CartId", cartId);

        return new ShoppingCart(context) { ShoppingCartId = cartId };
    }

    public void AddToCart(Pie pie)
    {
        var shoppingCartItem = _bethaniePieShopDbContext.ShoppingCartItems.SingleOrDefault(
            s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId
        );

        if (shoppingCartItem == null)
        {
            shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = ShoppingCartId,
                Pie = pie,
                Amount = 1
            };

            _bethaniePieShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
        }
        else
        {
            shoppingCartItem.Amount++;
        }

        _bethaniePieShopDbContext.SaveChanges();
    }

    public void ClearCart()
    {
        var cartItems = _bethaniePieShopDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId);

        _bethaniePieShopDbContext.ShoppingCartItems.RemoveRange(cartItems); // RemoveRange() is an extension method from Microsoft.EntityFrameworkCore, it removes all the items in the cart

        _bethaniePieShopDbContext.SaveChanges();
    }

    public List<ShoppingCartItem> GetShoppingCartItems()
    {
        return ShoppingCartItems ??= _bethaniePieShopDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId).Include(s => s.Pie).ToList();

    }

    public decimal GetShoppingCartTotal()
    {
        var total = _bethaniePieShopDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId).Select(cart => cart.Pie.Price * cart.Amount).Sum();
        return total;
    }

    public int RemoveFromCart(Pie pie)
    {
        var shoppingCartItem = _bethaniePieShopDbContext.ShoppingCartItems.SingleOrDefault(
            s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId
        );

        var localAmount = 0;

        if (shoppingCartItem != null)
        {
            if (shoppingCartItem.Amount > 1)
            {
                shoppingCartItem.Amount--;
                localAmount = shoppingCartItem.Amount;
            }
            else
            {
                _bethaniePieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
            }
        }

        _bethaniePieShopDbContext.SaveChanges();

        return localAmount;
    }
}
