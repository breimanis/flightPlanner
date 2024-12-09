using FlightPlanner.Database;
using FlightPlanner.Models;
using FlightPlanner.Utils;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Storage
{
    public class FlightStorage(FlightPlannerDbContext context)
    {
        private readonly FlightPlannerDbContext _dbContext = context;

        private static readonly object lockObject = new object();
        public Flight AddFlight(Flight flight)
        {
            lock (lockObject)
            {
                _dbContext.Flights.Add(flight);

                if (FlightValidator.ValidAirport(flight.From) && FlightValidator.ValidAirport(flight.To) && AirportAlreadyInList(flight.From) && AirportAlreadyInList(flight.To))
                {
                    _dbContext.Airports.Add(flight.From);
                    _dbContext.Airports.Add(flight.To);
                    _dbContext.SaveChanges();
                }

                _dbContext.SaveChanges();
                return flight;
            }
        }

        public Flight? GetFlight(int id)
        {
            return _dbContext.Flights.Include(f => f.From).Include(f => f.To).FirstOrDefault(x => x.Id == id);
        }

        public void ClearFlights()
        {
            _dbContext.Flights.RemoveRange(_dbContext.Flights.ToList());
            _dbContext.Airports.RemoveRange(_dbContext.Airports.ToList());
            _dbContext.SaveChanges();
        }

        public void DeleteFlight(int id)
        {
            var itemToRemove = _dbContext.Flights.FirstOrDefault(x => x.Id == id);

            if (itemToRemove == null)
                return;

            _dbContext.Flights.Remove(itemToRemove);
            context.SaveChanges();
        }

        public bool DoesFlightExist(Flight flight) // sis parak messy (?)
        {
            return _dbContext.Flights.Any(x =>
                x.From.AirportCode == flight.From.AirportCode &&
                x.To.AirportCode == flight.To.AirportCode &&
                x.Carrier == flight.Carrier &&
                x.DepartureTime == flight.DepartureTime &&
                x.ArrivalTime == flight.ArrivalTime);
        }

        public Flight[] SearchFlightFromRequest(SearchFlightRequest request)
        {
            var flightList = _dbContext.Flights.Where(
                x => x.From.AirportCode == request.From &&
                x.To.AirportCode == request.To)
                .ToArray();

            return flightList.Any() ? flightList : new Flight[0];
        }

        public bool AirportAlreadyInList(Airport airport)
        {
            return _dbContext.Airports.Any(x => x.City == airport.City && x.Country == airport.Country && x.AirportCode == airport.AirportCode);
        }
    }
}
