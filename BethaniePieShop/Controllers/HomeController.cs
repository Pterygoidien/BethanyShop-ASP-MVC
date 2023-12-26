using BethaniePieShop.Models;
using BethaniePieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethaniePieShop.Controllers;

public class HomeController : Controller
{
    private readonly IPieRepository _pieRepository;
    public HomeController(IPieRepository pieRepository)
    {
        this._pieRepository = pieRepository;
    }
    public IActionResult Index()
    {
        var piesOfTheWeek = _pieRepository.PiesOfTheWeek;
        var homeViewModel = new HomeViewModel(piesOfTheWeek);
        return View(homeViewModel);
    }

}
