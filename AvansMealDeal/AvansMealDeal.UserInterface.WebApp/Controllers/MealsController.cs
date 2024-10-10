using AvansMealDeal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvansMealDeal.UserInterface.WebApp.Controllers
{
    public class MealsController : Controller
    {
        private readonly IMealService mealService;

        public MealsController(IMealService mealService)
        {
            this.mealService = mealService;
        }

        public async Task<IActionResult> Index()
        {
            var meals = await mealService.GetAll();
            return View("Index", meals);
        }
    }
}
