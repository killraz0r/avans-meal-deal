using AvansMealDeal.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace AvansMealDeal.UserInterface.WebApp.Models
{
    public class AddMealViewModel
    {
        [Required(ErrorMessage = "Naam van de maaltijd ontbreekt")]
        [DataType(DataType.Text)]
        [Display(Name = "Naam")]
        public string Name { get; set; } = string.Empty; // fallback to prevent null reference error

        [Required(ErrorMessage = "Bevat de maaltijd alcohol?")]
        [Display(Name = "Bevat alcohol")]
        public bool ContainsAlcohol { get; set; } // makes meal for adults only
        
        public IFormFile? Photo { get; set; }

        // convert view model to model
        public Meal GetModel()
        {
            // read photo if given as input
            using var memoryStream = new MemoryStream();
            if (Photo != null)
            {
                Photo.CopyTo(memoryStream);
            }
         
            return new Meal
            {
                Name = Name,
                ContainsAlcohol = ContainsAlcohol,
                Photo = Photo != null 
                    ? memoryStream.ToArray() // photo given
                    : null                   // no photo
            };
        }
    }
}
