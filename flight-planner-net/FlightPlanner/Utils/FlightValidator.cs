using FlightPlanner.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;

namespace FlightPlanner.Utils
{
    public static class FlightValidator
    {
        public static bool IsValidFlight(Flight flight)
        {
            bool airportsValid = !AreAirportsEqual(flight.From, flight.To);
            bool bothAirportsAreValid = ValidAirport(flight.From) && ValidAirport(flight.To);
            bool flightDataValid = (!String.IsNullOrEmpty(flight.ArrivalTime) && !String.IsNullOrEmpty(flight.DepartureTime) && !String.IsNullOrEmpty(flight.Carrier.Trim()));
            bool areDatesCorrect = AreDatesValid(flight.DepartureTime, flight.ArrivalTime);

            return airportsValid && flightDataValid && areDatesCorrect && bothAirportsAreValid;
        }

        public static bool ValidAirport(Airport? airport) => (airport != null && !String.IsNullOrEmpty(airport.Country) && !String.IsNullOrEmpty(airport.City) && !String.IsNullOrEmpty(airport.AirportCode));

        public static bool AreAirportsEqual(Airport a, Airport b)
        {
            bool areEqualCaseInsensitive = (
                string.Equals(a.City.Trim(), b.City.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.Country.Trim(), b.Country.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.AirportCode.Trim(), b.AirportCode.Trim(), StringComparison.OrdinalIgnoreCase)
                );

            return areEqualCaseInsensitive;
        }

        public static bool AreDatesValid(string departure, string arrival)
        {
            var departureDate = DateTime.ParseExact(departure, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            var arrivalDate = DateTime.ParseExact(arrival, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            return arrivalDate > departureDate;
        }
    }
}