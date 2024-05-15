using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;
using TravelingTrips.Models;
using TravelingTrips.Pages.TravelComponents;
using TravelingTrips.Pages.TravelComponents.Print;

namespace TravelingTrips.Pages;

public partial class Home
{
    private User currentUser;
    private List<Travel> _travels = new List<Travel>();
    private ElementReference mapElement;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(userId))
        {
            currentUser = await Context.Users.FindAsync(userId);
            if (currentUser != null)
            {
                _travels = await Context.Travels
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitializeMapAsync();
        }
    }

    private async Task InitializeMapAsync()
    {
        try {
            await js.InvokeVoidAsync("initializeMap", mapElement);
        } catch (Exception ex) {
            Console.WriteLine($"Error initializing map: {ex.Message}");
        }
    }

    private void ReloadPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    private void DownloadFile(string filename)
    {
        var pdf = new PDFGenerator();
        pdf.DownloadPDF(js, filename);
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
                Snackbar.Add("Trip was successfully deleted");
            }

            ReloadPage();
        }
    }
    
    private void NavigateToDetails(int id)
    {
        NavigationManager.NavigateTo($"travelpage/{id}");
    }
}