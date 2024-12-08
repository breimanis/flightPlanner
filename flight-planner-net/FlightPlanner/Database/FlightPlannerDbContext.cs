using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;

namespace FlightPlanner.Database
{
    public class FlightPlannerDbContext(DbContextOptions<FlightPlannerDbContext> options) : DbContext(options)
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }
    }
}
