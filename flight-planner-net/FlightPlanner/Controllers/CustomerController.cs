using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports(string search) => Ok(AirportStorage.SearchAirports(search));

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightRequest req)
        {
            Flight[] flights = FlightStorage.SearchFlightFromRequest(req);

            if (req.From == req.To)
                return BadRequest();

            return Ok(new PageResult
            {
                page = 0, // kas ir sis?
                totalItems = flights.Length,
                items = flights
            });
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id) => FlightStorage.GetFlight(id) == null ? NotFound() : Ok(FlightStorage.GetFlight(id));
    }
}
