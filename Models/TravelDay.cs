namespace TravelingTrips.Models;

public class TravelDay
{
    public int Id { get; set; }
    public int? TravelId { get; set; }
    public DateTime? Date { get; set; }
    public Travel Travel { get; set; }
    public ICollection<TourismObject> TourismObjects { get; set; }
}