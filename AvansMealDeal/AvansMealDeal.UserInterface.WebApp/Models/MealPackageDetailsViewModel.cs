using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AvansMealDeal.UserInterface.WebApp.Models
{
    public class MealPackageDetailsViewModel
    {
		// all meals in the system
		public ICollection<Meal> SystemMeals { get; set; } = new List<Meal>(); // fallback to prevent null reference error

		// meals that are part of the meal package
		public ICollection<Meal> SelectedMeals { get; set; } = new List<Meal>(); // fallback to prevent null reference error

		public required int Id { get; set; }

		[Required(ErrorMessage = "Naam van het maaltijdpakket ontbreekt")]
		[DataType(DataType.Text)]
		[Display(Name = "Naam")]
		public string Name { get; set; } = string.Empty; // fallback to prevent null reference error

		[Required(ErrorMessage = "Prijs van het maaltijdpakket ontbreekt")]
		[DataType(DataType.Currency)]
		[Range(0.01, 9999.99, ErrorMessage = "Prijs van het maaltijdpakket moet tussen €0,01 en €9999,99 liggen")]
		[Display(Name = "Prijs")]
		public decimal? Price { get; set; }

		[Required(ErrorMessage = "Type van het maaltijdpakket ontbreekt")]
		[Display(Name = "Type")]
		public MealPackageType MealPackageType { get; set; }

		[Required(ErrorMessage = "Ophaaltijdstip van het maaltijdpakket ontbreekt")]
		[DataType(DataType.DateTime)]
		[Display(Name = "Ophaaltijdstip")]
		[PickupDeadlineValidation]
		public DateTimeOffset? PickupDeadline { get; set; }

		// if this is not null, a reservation was placed on the meal package
		public int? ReservationId { get; set; }
		public bool HasReservation => ReservationId != null;

		// convert view model to model
		public MealPackage GetModel()
		{
			return new MealPackage
			{
				Id = Id,
				Name = Name,
				Price = (decimal)Price!, // cannot be null if valid
				MealPackageType = MealPackageType,
				PickupDeadline = (DateTimeOffset)PickupDeadline!, // cannot be null if valid
			};
		}
	}
}
