using Microsoft.AspNetCore.Components;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelCreate : ComponentBase
{
    private bool _success;
    
    private string[] _errors = Array.Empty<string>();

    private MudForm? _form;

    [Parameter]
    public int? Id { get; set; } = null;

    [SupplyParameterFromQuery]
    private Travel CurrentTrip { get; set; } = new();

    private async Task CreateTrip()
    {
        Context.Travels.Add(CurrentTrip);
        
        await Context.SaveChangesAsync();

        Snackbar.Add("Trip was successfully created");
    }
}