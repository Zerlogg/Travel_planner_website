using Microsoft.EntityFrameworkCore;
using TravelingTrips.Models;

namespace TravelingTrips.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VideoGame>().HasData(
            new VideoGame { Id = 1, Publisher = "Emils", Title = "Osas", ReleaseYear = 2005},
            new VideoGame { Id = 2, Publisher = "Alex", Title = "Basas", ReleaseYear = 2001},
            new VideoGame { Id = 3, Publisher = "Ilja", Title = "Kasas", ReleaseYear = 2002}
        );
    }

    public DbSet<VideoGame> VideoGames { get; set; }
}