using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelingTrips.Models;

namespace TravelingTrips.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Folder> Folders { get; set; }
    
    public DbSet<Travel> Travels { get; set; }
    
    public DbSet<Accommodation> Accommodations { get; set; }
    
    public DbSet<Restaurant> Restaurants { get; set; }
    
    public DbSet<TravelDay> TravelDays { get; set; }
    
    public DbSet<TourismObject> TourismObjects { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<TravelFolders> TravelFolders { get; set; }
    
    public DbSet<MapPin> MapPins { get; set; }
}