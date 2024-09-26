namespace AvansMealDeal.Domain.Models
{
    // canteen meal package can be bought from
    public class Canteen
    {
        public int Id { get; set; }
        public required City City { get; set; }
        public required string Address { get; set; }
        public required bool OffersHotMeals { get; set; } // required to serve any HotMeal meals

        // meal package data
        public ICollection<MealPackage> MealPackages { get; set; } = [];
    }
}
