using FlightPlanner.Database;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController(FlightPlannerDbContext context, FlightStorage storage) : ControllerBase
    {
        private readonly FlightPlannerDbContext _dbContext = context;
        private readonly FlightStorage _storage = storage;

        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports(string search)
        {
            search = search.Trim().ToLower();
            var xd = _dbContext.Airports
                .Where(x =>
                    x.City.ToLower().Trim().Contains(search) ||
                    x.Country.ToLower().Trim().Contains(search) ||
                    x.AirportCode.ToLower().Trim().Contains(search))
                .ToArray();

            if (xd.Length < 1)
                return BadRequest();

            return Ok(xd);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightRequest req)
        {
            Flight[] flights = _storage.SearchFlightFromRequest(req);

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
        public IActionResult FindFlightById(int id) => _storage.GetFlight(id) == null ? NotFound() : Ok(_storage.GetFlight(id));
    }
}
