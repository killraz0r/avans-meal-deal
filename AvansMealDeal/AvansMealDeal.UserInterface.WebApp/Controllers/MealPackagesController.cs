using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.UserInterface.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvansMealDeal.UserInterface.WebApp.Controllers
{
    public class MealPackagesController : Controller
    {
        private readonly UserManager<MealDealUser> userManager;
        private readonly IMealPackageService mealPackageService;
        private readonly IMealService mealService;

        public MealPackagesController(UserManager<MealDealUser> userManager, IMealPackageService mealPackageService, IMealService mealService)
        {
            this.userManager = userManager;
            this.mealPackageService = mealPackageService;
            this.mealService = mealService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // stop if user is not logged in
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            var user = await userManager.FindByIdAsync(userId);
            // stop if user is not an employee
            if (user == null || !await userManager.IsInRoleAsync(user, Role.Employee))
            {
                return RedirectToAction("Index", "Home");
            }

            var mealPackages = await mealPackageService.GetForCanteen((int)user.EmployeeCanteenId!); // employee canteen id is not null if user is employee
            return View("Index", mealPackages);
        }

		[HttpGet]
		public async Task<IActionResult> Add()
		{
            var meals = await mealService.GetAll();
			return View("Add", new AddMealPackageViewModel { Meals = meals });
		}

		[HttpPost]
        public async Task<IActionResult> Add(AddMealPackageViewModel mealPackage, List<int> mealIds)
        {
			if (!ModelState.IsValid)
			{
				return await Add();
			}

            // get canteen the meal package will be sold in from the employee
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await userManager.FindByIdAsync(userId!);
            var canteenId = (int)user!.EmployeeCanteenId!; // canteen id cannot be null here
			
            await mealPackageService.Add(mealPackage.GetModel(canteenId), mealIds);
			return await Index();
		}
    }
}
