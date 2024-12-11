using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.UserInterface.WebService.Models
{
    public class ReservationPostBody
    {
        public required int MealPackageId { get; set; }
        public required string StudentId { get; set; }
        public required DateTimeOffset PlannedPickup { get; set; }

        public Reservation GetModel()
        {
            return new Reservation
            {
                MealPackageId = MealPackageId,
                StudentId = StudentId,
                PlannedPickup = PlannedPickup
            };
        }
    }
}
