using Microsoft.AspNetCore.Identity;

namespace AvansMealDeal.Domain.Models
{
    // additional info about a user, extends the identity info
    public class MealDealUser : IdentityUser
    {
        public required string Name { get; set; }

        // student only
        public string? StudentNumber { get; set; }
        public DateOnly? StudentBirthDate { get; set; }
        public City? StudentCity { get; set; }

        // employee only
        public string? EmployeeNumber { get; set; }
        public int? EmployeeCanteenId { get; set; }

    }
}
