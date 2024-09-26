namespace AvansMealDeal.Domain.Models
{
    // allows meals to be in multiple packages and packages to have multiple meals
    public class MealPackageItem
    {
        public int MealId { get; set; }
        public Meal Meal { get; set; } = null!;
        public bool AdultsOnly() => Meal.ContainsAlcohol;

        public int MealPackageId { get; set; }
        public MealPackage MealPackage { get; set; } = null!;
    }
}
