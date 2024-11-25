using FlightPlanner.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;

namespace FlightPlanner.Utils
{
    public static class FlightValidator
    {
        //any prop in Flight nullCheck, sim noteikti ir gatava metode, cireiz pameklesu
        public static bool ValidFlight(Flight flight)
        {
            var x = String.IsNullOrEmpty(flight.Carrier.Trim());
            bool invalidCheck = (String.IsNullOrEmpty(flight.ArrivalTime) || String.IsNullOrEmpty(flight.DepartureTime) || String.IsNullOrEmpty(flight.Carrier.Trim()));
            //r x = String.IsNullOrEmpty(flight.Carrier.Trim());
            return !invalidCheck;
        }
            
        //=> ( !String.IsNullOrEmpty(flight.ArrivalTime) && !String.IsNullOrEmpty(flight.DepartureTime) && !String.IsNullOrEmpty(flight.Carrier.Trim()));

        //any prop or Airport nullCheck
        public static bool ValidAirport(Airport? airport)
        {
            bool invalidCheck = ( airport == null || String.IsNullOrEmpty(airport?.Country) || String.IsNullOrEmpty(airport?.City) || String.IsNullOrEmpty(airport?.AirportCode) );
            return !invalidCheck;
        }
        //=> (airport != null && !String.IsNullOrEmpty(airport?.Country) && !String.IsNullOrEmpty(airport?.City) && !String.IsNullOrEmpty(airport?.AirportCode));
    
        public static bool AreAirportsEqual(Airport a, Airport b) // deep equality var velak uzmaukt
        {
            bool areEqualCaseInsensitive = (
                string.Equals(a.City.Trim(), b.City.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.Country.Trim(), b.Country.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.AirportCode.Trim(), b.AirportCode.Trim(), StringComparison.OrdinalIgnoreCase)
                );

            return areEqualCaseInsensitive;
        }
        
        public static bool InvalidDates(string departure, string arrival)
        {
            //DateTime parsedDepartureTime, parsedArrivalTime;
            var departureDate = DateTime.ParseExact(departure, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            var arrivalDate = DateTime.ParseExact(arrival, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            return departureDate >= arrivalDate;
        }

    }
}
