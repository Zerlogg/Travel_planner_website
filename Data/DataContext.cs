using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Folder = TravelingTrips.Models.Folder;
using MapPin = TravelingTrips.Models.MapPin;
using Travel = TravelingTrips.Models.Travel;
using TravelFolders = TravelingTrips.Models.TravelFolders;

namespace TravelingTrips.Data;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Folder>().HasData(
            new Folder { Id = 1, Title = "France"}, 
            new Folder { Id = 2, Title = "America"}
        );

        modelBuilder.Entity<Travel>().HasData(
            new Travel { Id = 1, City = "Paris", Description = "I love Paris", StartDate = new DateTime(2024, 3, 16), EndDate = new DateTime(2024, 3, 23), Budget = 412.40, Accommodation = "Hotel", Preferences = "I wanna see Eiffel tower"},
            new Travel { Id = 2, City = "Washington", Description = "I love Washington", StartDate = new DateTime(2024, 6, 24), EndDate = new DateTime(2024, 3, 31), Budget = 500.20, Accommodation = "Camping", Preferences = "I wanna see the White House"}
        );
    }

    public DbSet<Folder> Folders { get; set; }
    
    public DbSet<Travel> Travels { get; set; }
    
    public DbSet<TravelFolders> TravelFolders { get; set; }
    
    public DbSet<MapPin> MapPins { get; set; }
}