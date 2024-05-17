namespace TravelingTrips.Models;

public class Restaurant
{
    public int Id { get; set; }
    public int? TravelId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public TimeSpan? StartHours { get; set; }
    public TimeSpan? EndHours { get; set; }
    public bool? IsSelected { get; set; }
    public byte[] Image { get; set; }
    public Travel Travel { get; set; }
}