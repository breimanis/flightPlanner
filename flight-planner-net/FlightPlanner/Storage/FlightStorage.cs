using FlightPlanner.Database;
using FlightPlanner.Models;
using FlightPlanner.Utils;

namespace FlightPlanner.Storage
{
    public class FlightStorage(FlightPlannerDbContext context)
    {
        private readonly FlightPlannerDbContext _dbContext = context;
        private static readonly object lockObject = new object();
        private static List<Flight> _flights = new List<Flight>();

        public Flight AddFlight(Flight flight)
        {
            lock (lockObject)
            {
                //_flights.Add(flight);
                _dbContext.Flights.Add(flight);
                _dbContext.SaveChanges();


                if (FlightValidator.ValidAirport(flight.From) && FlightValidator.ValidAirport(flight.To) && !AirportStorage.AirportAlreadyInList(flight.From))
                    AirportStorage.AddAirport(flight.From);
                if (FlightValidator.ValidAirport(flight.From) && FlightValidator.ValidAirport(flight.To) && !AirportStorage.AirportAlreadyInList(flight.To))
                    AirportStorage.AddAirport(flight.To);

                return flight;
            }
        }

        public static Flight? GetFlight(int id) => _flights.FirstOrDefault(x => x.Id == id);
        public void ClearFlights()
        {
            _dbContext.Flights.RemoveRange(_dbContext.Flights);
            _dbContext.Airports.RemoveRange(_dbContext.Airports);
            //_flights.Clear();
            //AirportStorage.ClearAirports(); // sis diezgan ify,search flight by incomplete test failo citadak
        }
        public static void DeleteFlight(int id) => _flights.RemoveAll(x => x.Id == id);

        public static bool DoesFlightExist(Flight flight) // sis parak messy (?)
        {
            return _flights.Where(x =>
                x.From.AirportCode == flight.From.AirportCode &&
                x.To.AirportCode == flight.To.AirportCode &&
                x.Carrier == flight.Carrier &&
                x.DepartureTime == flight.DepartureTime &&
                x.ArrivalTime == flight.ArrivalTime)
                .Any();
        }


        public static Flight[] SearchFlightFromRequest(SearchFlightRequest request)
        {
            var flightList = _flights.Where(
                x => x.From.AirportCode == request.From &&
                x.To.AirportCode == request.To)
                .ToArray();

            return flightList.Any() ? flightList : new Flight[0];
        }

        public static PageResult CreatePageResult(List<Flight> flights)
        {
            return new PageResult
            {
                page = 0,
                totalItems = flights.Count,
                items = flights.ToArray()
            };
        }
    }
}
