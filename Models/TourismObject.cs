namespace TravelingTrips.Models;

public class TourismObject
{
    public int Id { get; set; }
    public string? TravelDayId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public double? Price { get; set; }
    public byte[] Image { get; set; }
}