using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class PageResult
    {
        public int page {  get; set; }
        public int totalItems { get; set; }
        public Flight[] items { get; set; }
    }
}
