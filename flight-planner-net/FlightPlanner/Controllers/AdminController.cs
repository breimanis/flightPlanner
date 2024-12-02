using FlightPlanner.Models;
using FlightPlanner.Storage;
using FlightPlanner.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private static readonly object lockObject = new object(); 
        //https://medium.com/@ipravn/achieving-efficient-concurrency-in-net-backend-a-comprehensive-guide-ab802fcb4dfc

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            lock (lockObject)
            {
                Flight? flight = FlightStorage.GetFlight(id);
                return flight == null ? NotFound() : Ok(flight);
            }
        }

        [Route("flights")]
        [HttpPost]
        public IActionResult AddFlight(Flight flight)
        {
            lock (lockObject)
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
        }


        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (lockObject)
            {
                FlightStorage.DeleteFlight(id);
                return Ok();
            }
        }
    }
}
