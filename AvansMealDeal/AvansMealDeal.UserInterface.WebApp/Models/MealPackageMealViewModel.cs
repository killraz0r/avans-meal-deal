using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.UserInterface.WebApp.Models
{
	public class MealPackageMealViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool ContainsAlcohol { get; set; } // makes meal for adults only
		public byte[]? Photo { get; set; }
		public bool Selected { get; set; } = false;

		public MealPackageMealViewModel(Meal meal, bool selected)
		{
			Id = meal.Id;
			Name = meal.Name;
			ContainsAlcohol = meal.ContainsAlcohol;
			Photo = meal.Photo;
			Selected = selected;
		}
	}
}
