using AvansMealDeal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvansMealDeal.UserInterface.WebService.Controllers
{
    [Route("mealpackage")]
    public class MealPackageController : ControllerBase
    {
        private readonly IMealPackageService mealPackageService;

        public MealPackageController(IMealPackageService mealPackageService)
        {
            this.mealPackageService = mealPackageService;
        }

        [HttpPost] // create
        public IActionResult Post()
        {
            return MethodNotAllowed();
        }

        [HttpGet] // read
        public async Task<IActionResult> Get()
        {
            var mealPackages = await mealPackageService.GetWithoutReservation();
            return Ok(mealPackages);
        }

        [HttpGet("{id}")] // read
        public async Task<IActionResult> GetById(int id)
        {
            var mealPackage = await mealPackageService.GetById(id);
            if (mealPackage == null)
            {
                return NotFound(new { Message = $"Maaltijdpakket {id} niet gevonden" });
            }
            return Ok(mealPackage);
        }

        [HttpPut("{id}")] // update
        public IActionResult Put(int id)
        {
            return MethodNotAllowed();
        }

        [HttpDelete("{id}")] // delete
        public IActionResult Delete(int id)
        {
            return MethodNotAllowed();
        }

        private IActionResult MethodNotAllowed()
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new { Message = "Maaltijdpakketten mogen alleen gelezen worden" });
        }
    }
}
