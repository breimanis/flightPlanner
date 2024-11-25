using FlightPlanner.Models;
using FlightPlanner.Storage;
using FlightPlanner.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            Flight? flight = FlightStorage.GetFlight(id);
            return flight == null ? NotFound() : Ok(flight);
        }

        [Route("flights")]
        [HttpPost]
        public IActionResult AddFlight(Flight flight)
        {
            if (FlightStorage.DoesFlightExist(flight))
                return Conflict();

            if (!FlightValidator.ValidAirport(flight.To) || 
                !FlightValidator.ValidAirport(flight.From) || 
                !FlightValidator.ValidFlight(flight) || 
                FlightValidator.AreAirportsEqual(flight.To, flight.From) ||
                FlightValidator.InvalidDates(flight.DepartureTime, flight.ArrivalTime))
                return BadRequest();

            FlightStorage.AddFlight(flight);
            
            return Created("", flight);
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            //if (FlightStorage.GetFlight(id) == null)
            //    return NotFound();

            FlightStorage.DeleteFlight(id);
            return Ok();
        }

    }
}
