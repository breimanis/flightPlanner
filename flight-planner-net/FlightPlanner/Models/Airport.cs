using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class Airport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string Country { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [JsonPropertyName("airport")]
        public string AirportCode { get; set; }
    }
}
