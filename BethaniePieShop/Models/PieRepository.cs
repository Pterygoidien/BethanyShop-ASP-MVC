

using Microsoft.EntityFrameworkCore;

namespace BethaniePieShop.Models;

public class PieRepository : IPieRepository
{
    private readonly BethaniePieShopDbContext _bethaniePieShopDbContext;

    public PieRepository(BethaniePieShopDbContext bethaniePieShopDbContext)
    {
        this._bethaniePieShopDbContext = bethaniePieShopDbContext;
    }

    public IEnumerable<Pie> AllPies => _bethaniePieShopDbContext.Pies.Include(c => c.Category);

    public IEnumerable<Pie> PiesOfTheWeek
    {
        get
        {
            return _bethaniePieShopDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
        }
    }

    public Pie? GetPieById(int pieId)
    {
        return _bethaniePieShopDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
    }
}
