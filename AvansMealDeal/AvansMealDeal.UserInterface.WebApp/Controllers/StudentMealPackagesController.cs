using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.UserInterface.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvansMealDeal.UserInterface.WebApp.Controllers
{
    public class StudentMealPackagesController : Controller
    {
        private readonly UserManager<MealDealUser> userManager;
        private readonly IMealPackageService mealPackageService;
        private readonly IReservationService reservationService;

        public StudentMealPackagesController(UserManager<MealDealUser> userManager, IMealPackageService mealPackageService, IReservationService reservationService)
        {
            this.userManager = userManager;
            this.mealPackageService = mealPackageService;
            this.reservationService = reservationService;
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

        public async Task<IActionResult> Details(int id)
        {
            var mealPackage = await mealPackageService.GetById(id);
            if (mealPackage == null)
            {
                return NotFound();
            }
            return View("Details", new StudentMealPackageDetailsViewModel { MealPackage = mealPackage } );
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(StudentMealPackageDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Details(model.Id);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            // check if student has made a reservation for the given date, prevent the reservation if true
            var plannedReservationDate = DateOnly.FromDateTime(model.PlannedPickup.Date);
            var studentHasMadeReservationForGivenDate = await reservationService.HasStudentMadeReservationForGivenDate(userId, plannedReservationDate);
            if (studentHasMadeReservationForGivenDate)
            {
                ModelState.AddModelError("PlannedPickup", $"Er is voor de gekozen dag {plannedReservationDate} al een reservering. Er is slechts een reservering per afhaaldag toegestaan.");
                return await Details(model.Id);
            }

            // check if student is an adult if a reservation is being made for an 18+ meal package
            var mealPackage = await mealPackageService.GetById(model.Id);
            if (mealPackage!.AdultsOnly())
            {
                var user = await userManager.FindByIdAsync(userId);
                var eighteenYearsAgoFromReservation = plannedReservationDate.AddYears(-18);

                if (user!.StudentBirthDate > eighteenYearsAgoFromReservation)
                {
                    ModelState.AddModelError("PlannedPickup", $"Je bent op {plannedReservationDate} nog niet volwassen, dus mag dit maaltijdpakket niet gereserveerd worden.");
                    return await Details(model.Id);
                }
            }

            await reservationService.Add(new Reservation
            { 
                StudentId = userId,
                MealPackageId = model.Id,
                PlannedPickup = model.PlannedPickup
            });
            return RedirectToAction("MyReservations");
        }
    }
}
