namespace TravelingTrips.Models;

public class TravelFolders
{
    public int Id { get; set; }
    public int FolderId { get; set; }
    public int TravelId { get; set; }
    public bool IsEnabled { get; set; } 
}