using Microsoft.AspNetCore.Components;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelDialog
{
    private bool _success;
    
    private string[] _errors = Array.Empty<string>();

    private MudForm? _form;
    
    [Parameter]
    public int? Id { get; set; }

    [SupplyParameterFromQuery]
    private Travel CurrentTrip { get; set; } = new();

    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

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

    private async Task EditTrip()
    {
        var dbTrip = await Context.Travels.FindAsync(Id);
        
        if (dbTrip != null)
        {
            dbTrip.City = CurrentTrip.City;
            dbTrip.Description = CurrentTrip.Description;
            dbTrip.Budget = CurrentTrip.Budget;
            dbTrip.Accommodation = CurrentTrip.Accommodation;
            dbTrip.Preferences = CurrentTrip.Preferences;

            await Context.SaveChangesAsync();
            
            StateHasChanged();
            
            Snackbar.Add("Trip was successfully saved");
        }

        MudDialog?.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog?.Cancel();
}