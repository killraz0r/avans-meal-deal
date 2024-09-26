namespace AvansMealDeal.Domain.Models
{
    // package of meals to be sold to students
    public class MealPackage
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required MealPackageType MealPackageType { get; set; }
        public required DateTimeOffset PickupDeadline { get; set; }

        // meal data
        public ICollection<MealPackageItem> Meals { get; set; } = [];
        public bool AdultsOnly() => Meals.Any(x => x.AdultsOnly());
        
        // canteen data
        public int CanteenId { get; set; }
        public Canteen Canteen { get; set; } = null!;

        // reservation data
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
