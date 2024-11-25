using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 0;

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = ++_id;
            _flights.Add(flight);

            if (flight.From != null && !AirportStorage.AirportAlreadyInList(flight.From))
                AirportStorage.AddAirport(flight.From);
            if (flight.To != null && !AirportStorage.AirportAlreadyInList(flight.To))
                AirportStorage.AddAirport(flight.To);

            return flight;
        }

        public static Flight? GetFlight(int id) => _flights.FirstOrDefault(x => x.Id == id);
        public static void ClearFlights() => _flights.Clear();
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
    }
}
