using Microsoft.EntityFrameworkCore;

namespace BethaniePieShop.Models;

public class BethaniePieShopDbContext : DbContext
{
    public BethaniePieShopDbContext(DbContextOptions<BethaniePieShopDbContext> options) : base(options)
    {
    }

    public DbSet<Pie> Pies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

}
