using FlightPlanner.Models;
using FlightPlanner.Utils;

namespace FlightPlanner.Storage
{
    public static class AirportStorage
    {
        private static List<Airport> _airports = new List<Airport>();
        public static void AddAirport(Airport airport)
        {
            if (!FlightValidator.ValidAirport(airport))
                return;

            _airports.Add(airport);
        }

        public static void ClearAirports()
        {
            _airports.Clear();
        }

        public static Airport? GetAirport(string airportCode) => _airports.FirstOrDefault(x => x.AirportCode == airportCode);
        public static bool AirportAlreadyInList(Airport airport) => _airports.Any(x => x.AirportCode == airport.AirportCode);

        public static Airport[] SearchAirports12(string value)
        {
            var value1 = value.Trim().ToLower();
            Airport[] airports = _airports
                .Where(x =>
                    x.City.ToLower().Trim().Contains(value1) ||
                    x.Country.ToLower().Trim().Contains(value1) ||
                    x.AirportCode.ToLower().Trim().Contains(value1))
                .ToArray();

            return airports;
        }
    }
}
