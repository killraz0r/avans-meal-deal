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

        public MealPackagesController(UserManager<MealDealUser> userManager, IMealPackageService mealPackageService)
        {
            this.userManager = userManager;
            this.mealPackageService = mealPackageService;
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
		public IActionResult Add()
		{
			return View("Add");
		}

		[HttpPost]
        public async Task<IActionResult> Add(AddMealPackageViewModel mealPackage)
        {
			if (!ModelState.IsValid)
			{
				return Add();
			}

            // get canteen the meal package will be sold in from the employee
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await userManager.FindByIdAsync(userId!);
            var canteenId = (int)user!.EmployeeCanteenId!; // canteen id cannot be null here
			
            await mealPackageService.Add(mealPackage.GetModel(canteenId));
			return await Index();
		}
    }
}
