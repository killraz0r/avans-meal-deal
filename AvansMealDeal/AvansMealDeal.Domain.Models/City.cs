using System.ComponentModel.DataAnnotations;

namespace AvansMealDeal.Domain.Models
{
    // enumeration of supported cities
    public enum City
    {
        [Display(Name = "Breda")]
        Breda = 1,

        [Display(Name = "Tilburg")]
        Tilburg = 2,

        [Display(Name = "Den Bosch")]
        DenBosch = 3
    }
}
