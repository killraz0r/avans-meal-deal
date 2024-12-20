﻿using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.UserInterface.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvansMealDeal.UserInterface.WebApp.Controllers
{
    public class EmployeeMealPackagesController : Controller
    {
        private readonly UserManager<MealDealUser> userManager;
        private readonly IMealPackageService mealPackageService;
        private readonly IMealService mealService;
        private readonly ICanteenService canteenService;

        public EmployeeMealPackagesController(UserManager<MealDealUser> userManager, IMealPackageService mealPackageService, IMealService mealService, ICanteenService canteenService)
        {
            this.userManager = userManager;
            this.mealPackageService = mealPackageService;
            this.mealService = mealService;
            this.canteenService = canteenService;
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

        public async Task<IActionResult> OtherCanteens()
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

            var mealPackages = await mealPackageService.GetForOtherCanteens((int)user.EmployeeCanteenId!); // employee canteen id is not null if user is employee
            return View("OtherCanteens", mealPackages);
        }

        public async Task<IActionResult> Details(int id)
        {
            var mealPackage = await mealPackageService.GetById(id);
            if (mealPackage == null)
            {
                return NotFound();
            }
			var systemMeals = await mealService.GetAll();
            var employeeCanteen = await canteenService.GetById(mealPackage.CanteenId);
            return View("Details", new EmployeeMealPackageDetailsViewModel 
            {
                SystemMeals = systemMeals,
                CanteenOffersHotMeals = employeeCanteen.OffersHotMeals,
                SelectedMeals = mealPackage.Meals.Select(x => x.Meal).ToList(),
                Id = mealPackage.Id,
                Name = mealPackage.Name,
                Price = mealPackage.Price,
                MealPackageType = mealPackage.MealPackageType,
                PickupDeadline = mealPackage.PickupDeadline,
                ReservationId = mealPackage.Reservation?.Id ?? mealPackage.ReservationId,
                CanteenId = mealPackage.Canteen?.Id ?? mealPackage.CanteenId,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeMealPackageDetailsViewModel mealPackageDetails, List<int> mealIds)
        {
			if (ModelState.IsValid)
			{
				await mealPackageService.Edit(mealPackageDetails.GetModel(), mealIds);
			}

			return await Details(mealPackageDetails.Id);
		}

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await mealPackageService.Remove(id);
            return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Add()
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

            var meals = await mealService.GetAll();
            var employeeCanteen = await canteenService.GetById(user.EmployeeCanteenId!.Value);
            return View("Add", new AddMealPackageViewModel { Meals = meals, CanteenOffersHotMeals = employeeCanteen.OffersHotMeals });
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
            return RedirectToAction("Index");
        }
    }
}
