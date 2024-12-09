using FlightPlanner.Database;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using FlightPlanner.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController(FlightStorage storage) : ControllerBase
    {
        private readonly FlightStorage _storage = storage;
        private static readonly object lockObject = new object(); 
        //https://medium.com/@ipravn/achieving-efficient-concurrency-in-net-backend-a-comprehensive-guide-ab802fcb4dfc

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            lock (lockObject)
            {
                Flight? flight = _storage.GetFlight(id);
                return flight == null ? NotFound() : Ok(flight);
            }
        }

        [Route("flights")]
        [HttpPost]
        public IActionResult AddFlight(Flight flight)
        {
            lock (lockObject)
            {
                if (_storage.DoesFlightExist(flight))
                    return Conflict();

                if (!FlightValidator.IsValidFlight(flight))
                    return BadRequest();

                _storage.AddFlight(flight);

                return Created("", flight);
            }
        }


        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (lockObject)
            {
                _storage.DeleteFlight(id);
                return Ok();
            }
        }
    }
}
