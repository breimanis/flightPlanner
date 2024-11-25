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
        public IActionResult SearchAirports(string search)
        {
            var ret = AirportStorage.SearchAirports(search);
            return Ok(ret);
        }

        [Route("flights/${id}")]
        [HttpGet]
        public IActionResult GetAirPortById(int id) => Ok(FlightStorage.GetFlight(id));
    }
}
