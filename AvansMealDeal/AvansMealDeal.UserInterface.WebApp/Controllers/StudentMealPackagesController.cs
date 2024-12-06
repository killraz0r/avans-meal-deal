using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvansMealDeal.UserInterface.WebApp.Controllers
{
    public class StudentMealPackagesController : Controller
    {
        private readonly UserManager<MealDealUser> userManager;
        private readonly IMealPackageService mealPackageService;

        public StudentMealPackagesController(UserManager<MealDealUser> userManager, IMealPackageService mealPackageService)
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
            // stop if user is not a student
            if (user == null || !await userManager.IsInRoleAsync(user, Role.Student))
            {
                return RedirectToAction("Index", "Home");
            }

            var mealPackages = await mealPackageService.GetWithoutReservation();
            return View("Index", mealPackages);
        }

        public async Task<IActionResult> MyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // stop if user is not logged in
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            // stop if user is not a student
            if (user == null || !await userManager.IsInRoleAsync(user, Role.Student))
            {
                return RedirectToAction("Index", "Home");
            }

            var mealPackages = await mealPackageService.GetWithReservationForStudent(user.Id);
            return View("MyReservations", mealPackages);
        }
    }
}
