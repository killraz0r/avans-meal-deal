using AvansMealDeal.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace AvansMealDeal.UserInterface.WebApp.Models
{
    public class AddMealPackageViewModel
    {
		[Required(ErrorMessage = "Naam van het maaltijdpakket ontbreekt")]
		[DataType(DataType.Text)]
		[Display(Name = "Naam")]
		public string Name { get; set; } = string.Empty; // fallback to prevent null reference error

		[Required(ErrorMessage = "Prijs van het maaltijdpakket ontbreekt")]
		[DataType(DataType.Currency)]
		[Display(Name = "Prijs")]
		public decimal? Price { get; set; }

		[Required(ErrorMessage = "Type van het maaltijdpakket ontbreekt")]
		[Display(Name = "Type")]
		public MealPackageType MealPackageType { get; set; }

		[Required(ErrorMessage = "Ophaaltijdstip van het maaltijdpakket ontbreekt")]
		[DataType(DataType.DateTime)]
		[Display(Name = "Ophaaltijdstip")]
		public DateTimeOffset? PickupDeadline { get; set; }

		// convert view model to model
		public MealPackage GetModel(int canteenId)
		{
			return new MealPackage
			{
				Name = Name,
				Price = (decimal)Price!, // cannot be null if valid
				MealPackageType = MealPackageType,
				PickupDeadline = (DateTimeOffset)PickupDeadline!, // cannot be null if valid
				CanteenId = canteenId // canteen id from employee
			};
		}
	}
}
