using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelCollection 
{
    private string? TextValue { get; set; }

    private List<Travel> _travels = new List<Travel>();
    
    protected override async Task OnInitializedAsync()
    {
        _travels = await Context.Travels.ToListAsync();
    }
    
    private void EditTravel(int id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var options = new DialogOptions { CloseOnEscapeKey = true };
        DialogService.Show<TravelDialog>("Edit trip", parameters, options);
    }
    
    private async Task DeleteTravel(int id)
    {
        var dbTravel = await Context.Travels.FindAsync(id);
        if (dbTravel != null)
        {
            Context.Remove(dbTravel);
            await Context.SaveChangesAsync();
            Snackbar.Add("Trip was successfully deleted");
        }
    }
    
    private void NavigateToDetails(int id)
    {
        NavigationManager.NavigateTo($"travelpage/{id}");
    }
}