using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelCollection 
{
    private string? TextValue { get; set; }

    private List<Travel> _travels = new List<Travel>();
    
    private List<Travel> _filteredTravels = new List<Travel>();
    
    async Task<string> getUserId(){
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var UserId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
        return UserId;
    }
    
    protected override async Task OnInitializedAsync()
    {
        await LoadTravels();
    }

    private async Task LoadTravels()
    {
        var userId = await getUserId();
        _travels = await Context.Travels.Where(x => x.UserId == userId).ToListAsync();
        ApplySearchFilter();
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
    
        
    private void ApplySearchFilter()
    {
        if (!string.IsNullOrWhiteSpace(TextValue))
        {
            _filteredTravels = _travels.Where(t => t.City != null && t.City.Contains(TextValue, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        else
        {
            _filteredTravels = new List<Travel>(_travels);
        }
    }
    
    private void SearchTravel()
    {
        ApplySearchFilter();
        StateHasChanged();
    }
}