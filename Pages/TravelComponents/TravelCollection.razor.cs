using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;
using TravelingTrips.Pages.TravelComponents.Print;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelCollection 
{
    private string? TextValue { get; set; }
    
    [Parameter]
    public Travel CurrentTrip { get; set; }

    private List<Travel> _travels = new List<Travel>();
    
    private List<Travel> _filteredTravels = new List<Travel>();
    
    protected override async Task OnInitializedAsync()
    {
        await LoadTravels();
    }
    
    private void ReloadPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    private async Task LoadTravels()
    {
        var userId = await UserService.GetUserId();
        _travels = await Context.Travels.Where(x => x.UserId == userId).ToListAsync();
        ApplySearchFilter();
    }
    
    private async Task DownloadFile(int travelId, string filename)
    {
        var pdf = new PDFGenerator();
        var currentTrip = await Context.Travels.FindAsync(travelId);
        var accommodations = await LoadAccommodations(travelId);
        var restaurants = await LoadRestaurants(travelId);
        var tourismObjects = await LoadTourismObjects(travelId);
        pdf.DownloadPDF(js, currentTrip, accommodations, restaurants, tourismObjects, filename);
    }
    
    private async Task<List<Accommodation>> LoadAccommodations(int travelId)
    {
        return await Context.Accommodations
            .Where(a => a.TravelId == travelId)
            .ToListAsync();
    }
    
    private async Task<List<Restaurant>> LoadRestaurants(int travelId)
    {
        return await Context.Restaurants
            .Where(a => a.TravelId == travelId)
            .ToListAsync();
    }
    
    private async Task<List<List<TourismObject>>> LoadTourismObjects(int travelId)
    {
        var travelDays = await Context.TravelDays
            .Where(td => td.TravelId == travelId)
            .ToListAsync();

        var tourismObjectsList = new List<List<TourismObject>>();

        foreach (var day in travelDays)
        {
            var tourismObjects = await Context.TourismObjects
                .Where(to => to.TravelDayId == day.Id)
                .ToListAsync();
        
            tourismObjectsList.Add(tourismObjects);
        }

        return tourismObjectsList;
    }
    
    private void EditTravel(int id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var options = new DialogOptions { CloseOnEscapeKey = true };
        DialogService.Show<TravelDialog>("Edit trip", parameters, options);
    }
    
    private async Task DeleteTravel(int id)
    {
        var result = await DialogService.ShowMessageBox("Delete Trip", "Are you sure you want to delete this trip?", yesText: "Yes", cancelText: "Cancel");

        if (result == true)
        {
            var dbTravel = await Context.Travels.FindAsync(id);
            if (dbTravel != null)
            {
                Context.Remove(dbTravel);
                await Context.SaveChangesAsync();
                ReloadPage();
                Snackbar.Add("Trip was successfully deleted");
            }
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