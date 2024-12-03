using System.ComponentModel.DataAnnotations;

namespace AvansMealDeal.Domain.Models
{
    // type of meal package
    public enum MealPackageType
    {
		[Display(Name = "Brood")]
		Bread = 1,

		[Display(Name = "Warme avondmaaltijd")]
		HotMeal = 2,

		[Display(Name = "Drank")]
		Drinks = 3
    }
}
