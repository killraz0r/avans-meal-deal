using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.UserInterface.WebApp.Models;
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

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMealViewModel meal)
        {
            if (!ModelState.IsValid)
            {
                return Add();
            }

            await mealService.Add(meal.GetModel());
            return await Index();
        }
    }
}
