using Microsoft.AspNetCore.Components;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelCreate : ComponentBase
{
    private bool _success;
    
    private string[] _errors = Array.Empty<string>();

    private MudForm? _form;
    
    private readonly DateTime _minDate = DateTime.Now.Date;

    [Parameter]
    public int? Id { get; set; } = null;

    [SupplyParameterFromQuery]
    private Travel CurrentTrip { get; set; } = new();
    
    async Task<string> getUserId(){
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var UserId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
        return UserId;
    }

    private async Task CreateTrip()
    {
        CurrentTrip.UserId = await getUserId();
        Context.Travels.Add(CurrentTrip);
        
        await Context.SaveChangesAsync();
        
        NavigationManager.NavigateTo("/travels");

        Snackbar.Add("Trip was successfully created");
    }
}