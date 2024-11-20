using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.UserInterface.WebApp.Models
{
    public class MealPackageDetailsViewModel
    {
		// all meals in the system
		public required ICollection<Meal> Meals { get; set; }

		// the meal package itself
		public required MealPackage MealPackage { get; set; }
	}
}
