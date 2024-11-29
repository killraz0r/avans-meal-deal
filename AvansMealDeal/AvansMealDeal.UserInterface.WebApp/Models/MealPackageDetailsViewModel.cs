using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.UserInterface.WebApp.Models
{
    public class MealPackageDetailsViewModel
    {
		// all meals in the system
		public ICollection<Meal> SystemMeals { get; set; } = new List<Meal>(); // fallback to prevent null reference error

		// meals that are part of the meal package
		public ICollection<Meal> SelectedMeals { get; set; } = new List<Meal>(); // fallback to prevent null reference error

		public int Id { get; set; }
		public required string Name { get; set; }
		public required decimal Price { get; set; }
		public required MealPackageType MealPackageType { get; set; }
		public required DateTimeOffset PickupDeadline { get; set; }

		// if this is not null, a reservation was placed on the meal package
		public int? ReservationId { get; set; }

		public MealPackage GetModel()
		{
			return new MealPackage
			{
				Id = Id,
				Name = Name,
				Price = Price,
				MealPackageType = MealPackageType,
				PickupDeadline = PickupDeadline,

			};
		}
	}
}
