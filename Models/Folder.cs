using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace TravelingTrips.Models;

public class Folder
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? Title { get; set; }

    [NotMapped]
    public bool IsEnabled { get; set; }
    
    public List<Travel> Travels = new List<Travel>();

    public override bool Equals(object o)
    {
        var other = o as Folder;
        return other?.Id == Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString()
    {
        return Title;
    }
}