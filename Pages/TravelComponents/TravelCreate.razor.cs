using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
    
    private async Task<string?> GetGoogleImage(string cityName)
    {
        try
        {
            return await js.InvokeAsync<string>("searchGoogleImages", cityName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error invoking JavaScript interop: {ex.Message}");
            return null;
        }
    }

    private async Task CreateTrip()
    {
        try
        {
            if (CurrentTrip.StartDate >= CurrentTrip.EndDate)
            {
                Snackbar.Add("End date must be later than the start date", Severity.Error);
                return;
            }

            CurrentTrip.UserId = await getUserId();
            CurrentTrip.Image = await GetGoogleImage(CurrentTrip.City);

            Context.Travels.Add(CurrentTrip);
            await Context.SaveChangesAsync();
            
            var travelDays = GenerateTravelDays(CurrentTrip);
            Context.TravelDays.AddRange(travelDays);
            await Context.SaveChangesAsync();

            NavigationManager.NavigateTo("/travels");
            Snackbar.Add("Trip was successfully created");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating trip: {ex.Message}");
            Snackbar.Add("An error occurred while creating the trip", Severity.Error);
        }
    }
    
    private IEnumerable<TravelDay> GenerateTravelDays(Travel trip)
    {
        var travelDays = new List<TravelDay>();
        if (trip.StartDate.HasValue && trip.EndDate.HasValue)
        {
            DateTime currentDate = trip.StartDate.Value;
            while (currentDate <= trip.EndDate.Value)
            {
                travelDays.Add(new TravelDay
                {
                    TravelId = trip.Id,
                    Date = currentDate
                });
                currentDate = currentDate.AddDays(1);
            }
        }
        return travelDays;
    }
}