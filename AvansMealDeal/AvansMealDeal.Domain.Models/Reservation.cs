namespace AvansMealDeal.Domain.Models
{
    // reservation a student can make for a meal package
    public class Reservation
    {
        public int Id { get; set; }

        public required int StudentId { get; set; }
        public required DateTimeOffset PlannedPickup { get; set; }

        // meal package data
        public int MealPackageId { get; set; }
        public MealPackage MealPackage { get; set; } = null!;
    }
}
