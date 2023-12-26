namespace BethaniePieShop.Models;

public interface ICategoryRepository
{
    IEnumerable<Category> AllCategories { get; }

}
