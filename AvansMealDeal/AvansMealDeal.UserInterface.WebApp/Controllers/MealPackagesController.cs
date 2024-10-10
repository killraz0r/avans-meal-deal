using Microsoft.AspNetCore.Mvc;

namespace AvansMealDeal.UserInterface.WebApp.Controllers
{
    public class MealPackagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
