using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;
using TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageDays.TravelDaysObjects;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageDays;

public partial class TravelDays
{
    [Parameter] 
    public int TravelId { get; set; }

    private List<TravelDay> travelDays { get; set; } = new List<TravelDay>();
    
    private void ReloadPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    protected override async Task OnParametersSetAsync()
    {
        await using var context = ContextFactory.CreateDbContext();
        travelDays = await context.TravelDays
            .Where(td => td.TravelId == TravelId)
            .ToListAsync();
    }

    private async Task OpenAddDialog(int travelDayId)
    {
        var parameters = new DialogParameters { ["TravelDayId"] = travelDayId };
        var dialog = DialogService.Show<TravelTourismObjectsAdd>("Add Tourism Object", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await OnParametersSetAsync();
            ReloadPage();
        }
    }
}