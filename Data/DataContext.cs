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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Travel>()
            .HasMany(t => t.Accommodations)
            .WithOne(a => a.Travel)
            .HasForeignKey(a => a.TravelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Travel>()
            .HasMany(t => t.Restaurants)
            .WithOne(r => r.Travel)
            .HasForeignKey(r => r.TravelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Travel>()
            .HasMany(t => t.TravelDays)
            .WithOne(td => td.Travel)
            .HasForeignKey(td => td.TravelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TravelDay>()
            .HasMany(td => td.TourismObjects)
            .WithOne(to => to.TravelDay)
            .HasForeignKey(to => to.TravelDayId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}