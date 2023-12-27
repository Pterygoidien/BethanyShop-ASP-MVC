using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Models;

public class ShoppingCart : IShoppingCart
{
    private BethanysPieShopDbContext _BethanysPieShopDbContext;
    public string? ShoppingCartId { get; set; }

    private ShoppingCart(BethanysPieShopDbContext BethanysPieShopDbContext)
    {
        this._BethanysPieShopDbContext = BethanysPieShopDbContext;
    }

    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

    public static ShoppingCart getCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

        BethanysPieShopDbContext context = services.GetService<BethanysPieShopDbContext>() ?? throw new Exception("Error initializing database context");

        string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

        session?.SetString("CartId", cartId);

        return new ShoppingCart(context) { ShoppingCartId = cartId };
    }

    public void AddToCart(Pie pie)
    {
        var shoppingCartItem = _BethanysPieShopDbContext.ShoppingCartItems.SingleOrDefault(
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

            _BethanysPieShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
        }
        else
        {
            shoppingCartItem.Amount++;
        }

        _BethanysPieShopDbContext.SaveChanges();
    }

    public void ClearCart()
    {
        var cartItems = _BethanysPieShopDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId);

        _BethanysPieShopDbContext.ShoppingCartItems.RemoveRange(cartItems); // RemoveRange() is an extension method from Microsoft.EntityFrameworkCore, it removes all the items in the cart

        _BethanysPieShopDbContext.SaveChanges();
    }

    public List<ShoppingCartItem> GetShoppingCartItems()
    {
        return ShoppingCartItems ??= _BethanysPieShopDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId).Include(s => s.Pie).ToList();

    }

    public decimal GetShoppingCartTotal()
    {
        var total = _BethanysPieShopDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId).Select(cart => cart.Pie.Price * cart.Amount).Sum();
        return total;
    }

    public int RemoveFromCart(Pie pie)
    {
        var shoppingCartItem = _BethanysPieShopDbContext.ShoppingCartItems.SingleOrDefault(
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
                _BethanysPieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
            }
        }

        _BethanysPieShopDbContext.SaveChanges();

        return localAmount;
    }
}
