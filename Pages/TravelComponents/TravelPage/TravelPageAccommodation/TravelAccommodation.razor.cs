using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageAccommodation;

public partial class TravelAccommodation
{
    [Parameter]
    public Travel CurrentTrip { get; set; }
    
    private bool AlarmOn { get; set; }

    private List<Accommodation> accommodations;

    private async Task OpenAddDialog()
    {
        var parameters = new DialogParameters();
        parameters.Add("CurrentTrip", CurrentTrip);

        var dialog = DialogService.Show<TravelAccommodationAdd>("Add Accommodation", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await LoadAccommodations();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadAccommodations();
    }
    
    private async Task ToggleAccommodation(Accommodation accommodation, bool toggled)
    {
        if (accommodation != null)
        {
            accommodation.IsSelected = toggled;
            
            Context.Update(accommodation);
            await Context.SaveChangesAsync();
        }
    }

    private async Task LoadAccommodations()
    {
        if (CurrentTrip != null)
        {
            accommodations = await Context.Accommodations
                .Where(a => a.TravelId == CurrentTrip.Id.ToString())
                .ToListAsync();
            
            foreach (var accommodation in accommodations)
            {
                AlarmOn = (bool)accommodation.IsSelected;
            }
        }
    }
    
    private async Task DeleteAccommodation(Accommodation accommodation)
    {
        if (accommodation != null)
        {
            Context.Accommodations.Remove(accommodation);
            await Context.SaveChangesAsync();
            await LoadAccommodations();
            Snackbar.Add("Accommodation deleted successfully");
        }
    }
    
    private async Task OpenEditDialog(Accommodation accommodation)
    {
        var parameters = new DialogParameters();
        parameters.Add("Accommodation", accommodation);

        var dialog = DialogService.Show<TravelAccommodationEdit>("Edit Accommodation", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await LoadAccommodations();
            Snackbar.Add("Accommodation updated successfully");
        }
    }
}