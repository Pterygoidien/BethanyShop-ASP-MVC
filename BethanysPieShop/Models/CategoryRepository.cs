namespace BethanysPieShop.Models;

public class CategoryRepository : ICategoryRepository
{
    private readonly BethanysPieShopDbContext _BethanysPieShopDbContext;

    public CategoryRepository(BethanysPieShopDbContext BethanysPieShopDbContext)
    {
        this._BethanysPieShopDbContext = BethanysPieShopDbContext;
    }

    public IEnumerable<Category> AllCategories => _BethanysPieShopDbContext.Categories.OrderBy(c => c.CategoryName);
}
