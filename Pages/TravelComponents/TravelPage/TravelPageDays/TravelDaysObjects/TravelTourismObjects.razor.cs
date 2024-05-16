using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageDays.TravelDaysObjects;

public partial class TravelTourismObjects : ComponentBase
{
    [Parameter]
    public int TravelDayId { get; set; }

    private List<TourismObject> tourismObjects = new List<TourismObject>();

    protected override async Task OnParametersSetAsync()
    {
        await LoadTourismObjects();
    }
    
    private async Task LoadTourismObjects()
    {
        await using var context = ContextFactory.CreateDbContext();
        tourismObjects = await context.TourismObjects
            .Where(to => to.TravelDayId == TravelDayId.ToString())
            .ToListAsync();
    }

    private async Task OpenEditDialog(TourismObject tourismObject)
    {
        var parameters = new DialogParameters { ["tourismObject"] = tourismObject };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small};
        var dialog = DialogService.Show<TravelTourismObjectsEdit>("Edit Tourism Object", parameters, options);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await LoadTourismObjects();
        }
        else
        {
            await LoadTourismObjects();
        }
    }

    private async Task DeleteTourismObject(TourismObject tourismObject)
    {
        var confirmed = await DialogService.ShowMessageBox("Delete Tourism Object", 
            $"Are you sure you want to delete {tourismObject.Name}?", 
            yesText: "Yes", noText: "Cancel");
        
        if (confirmed == true)
        {
            Context.TourismObjects.Remove(tourismObject);
            await Context.SaveChangesAsync();
            tourismObjects.Remove(tourismObject);
            Snackbar.Add("Tourism Object deleted successfully");
        }
    }
}