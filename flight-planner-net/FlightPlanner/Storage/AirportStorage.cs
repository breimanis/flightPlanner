using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public static class AirportStorage
    {
        private static List<Airport> _airports = new List<Airport>();
        public static void AddAirport(Airport airport) => _airports.Add(airport);
        public static Airport? GetAirport(string airportCode) => _airports.FirstOrDefault(x => x.AirportCode == airportCode);
        public static bool AirportAlreadyInList(Airport airport) => _airports.Any(x => x.AirportCode == airport.AirportCode);

        public static List<Airport> SearchAirports(string value)
        {
            value = value.Trim().ToLower();
            List<Airport> airports = _airports.Where(
                x => x.City.ToLower().Contains(value) ||
                x.Country.ToLower().Contains(value) ||
                x.AirportCode.ToLower().Contains(value))
                .ToList();

            return airports;
        }
    }
}
