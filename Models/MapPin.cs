namespace TravelingTrips.Models;

public class MapPin
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? UserId { get; set; }
}