using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AvansMealDeal.UserInterface.WebApp.Models
{
    public class StudentMealPackageDetailsViewModel
    {
        public MealPackage? MealPackage { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Ophaaltijdstip van het maaltijdpakket ontbreekt")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Ophaaltijdstip")]
        [PickupDeadlineValidation]
        public DateTimeOffset PlannedPickup { get; set; }
    }
}
