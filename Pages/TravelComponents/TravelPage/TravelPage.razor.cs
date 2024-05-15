using Microsoft.AspNetCore.Components;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage;

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
                CurrentTrip = travel;
            }
        }
    }
}