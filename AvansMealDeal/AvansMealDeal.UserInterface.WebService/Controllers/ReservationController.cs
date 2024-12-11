using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.UserInterface.WebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AvansMealDeal.UserInterface.WebService.Controllers
{
    // Richardson Maturity Model level (RMM) 2 endpoint for reservations
    [Route("reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpPost] // create
        public async Task<IActionResult> Post([FromBody] ReservationPostBody reservation)
        {
            try
            {
                await reservationService.Add(reservation.GetModel());
            }
            catch (Exception ex) 
            {
                return BadRequest(new { Message = ex.Message });
            }
            return NoContent();
        }

        [HttpGet] // read
        public IActionResult Get()
        {
            return MethodNotAllowed();
        }

        [HttpGet("{id}")] // read
        public IActionResult GetById([FromRoute] int id)
        {
            return MethodNotAllowed();
        }

        [HttpPut("{id}")] // update
        public IActionResult Put([FromRoute] int id)
        {
            return MethodNotAllowed();
        }

        [HttpDelete("{id}")] // delete
        public IActionResult Delete([FromRoute] int id)
        { 
            return MethodNotAllowed(); 
        }

        private IActionResult MethodNotAllowed()
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed, new { Message = "Reserveringen mogen alleen aangemaakt worden" });
        }
    }
}
