using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController(FlightStorage storage) : ControllerBase
    {
        private readonly FlightStorage _storage = storage;

        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            _storage.ClearFlights();
            return Ok();
        }
    }
}
