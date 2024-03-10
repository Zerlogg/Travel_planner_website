using Microsoft.AspNetCore.Components;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelPage : ComponentBase
{
    private bool _open = true;
    private string _selectedPage = "Home";
    
    private void OnNavLinkClick(string page)
    {
        _selectedPage = page;
    }
    
    [Parameter]
    public int? Id { get; set; } = null;

    [SupplyParameterFromQuery]
    private Travel CurrentTrip { get; set; } = new();
    
    protected override async Task OnParametersSetAsync()
    {
        if (Id != null)
        {
            var travel = await Context.Travels.FindAsync(Id);

            if (travel != null)
            {
                CurrentTrip.City ??= travel.City;
                CurrentTrip.Description ??= travel.Description;
                CurrentTrip.StartDate ??= travel.StartDate;
                CurrentTrip.EndDate ??= travel.EndDate;
                CurrentTrip.Budget ??= travel.Budget;
                CurrentTrip.Accommodation ??= travel.Accommodation;
                CurrentTrip.Preferences ??= travel.Preferences;
            }
        }
    }
}