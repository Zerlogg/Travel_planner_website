namespace TravelingTrips.Models;

public class Travel
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double? Budget { get; set; }
    public string? Preferences { get; set; }
    public string? Image { get; set; }
    public string? ChatHistory { get; set; }
    public ICollection<Accommodation> Accommodations { get; set; }
    public ICollection<Restaurant> Restaurants { get; set; }
    public ICollection<TravelDay> TravelDays { get; set; }
}