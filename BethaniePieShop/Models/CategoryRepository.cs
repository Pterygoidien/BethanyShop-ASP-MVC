namespace BethaniePieShop.Models;

public class CategoryRepository : ICategoryRepository
{
    private readonly BethaniePieShopDbContext _bethaniePieShopDbContext;

    public CategoryRepository(BethaniePieShopDbContext bethaniePieShopDbContext)
    {
        this._bethaniePieShopDbContext = bethaniePieShopDbContext;
    }

    public IEnumerable<Category> AllCategories => _bethaniePieShopDbContext.Categories.OrderBy(c => c.CategoryName);
}
