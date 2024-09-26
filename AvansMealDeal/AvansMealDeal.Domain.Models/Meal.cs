namespace AvansMealDeal.Domain.Models
{
    // info about a meal
    public class Meal
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required bool ContainsAlcohol { get; set; } // makes meal for adults only
        public byte[]? Photo { get; set; }

        public ICollection<MealPackageItem> MealPackages { get; set; } = [];
    }
}
