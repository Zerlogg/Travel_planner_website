using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageRestaurant;

public partial class TravelRestaurant
{
    [Parameter]
    public Travel CurrentTrip { get; set; }
    
    private bool AlarmOn { get; set; }

    private List<Restaurant> restaurants;

    private async Task OpenAddDialog()
    {
        var parameters = new DialogParameters();
        parameters.Add("CurrentTrip", CurrentTrip);

        var dialog = DialogService.Show<TravelRestaurantAdd>("Add Restaurant", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await LoadRestaurants();
        }
    }
    
    string FormatTimeSpan(TimeSpan? timeSpan)
    {
        if (timeSpan.HasValue)
        {
            return timeSpan.Value.ToString(@"hh\:mm");
        }
        else
        {
            return string.Empty;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadRestaurants();
    }
    
    private async Task ToggleRestaurant(Restaurant restaurant, bool toggled)
    {
        if (restaurant != null)
        {
            restaurant.IsSelected = toggled;
            
            Context.Update(restaurant);
            await Context.SaveChangesAsync();
        }
    }

    private async Task LoadRestaurants()
    {
        if (CurrentTrip != null)
        {
            restaurants = await Context.Restaurants
                .Where(a => a.TravelId == CurrentTrip.Id.ToString())
                .ToListAsync();
            
            foreach (var restaurant in restaurants)
            {
                AlarmOn = (bool)restaurant.IsSelected;
            }
        }
    }
    
    private async Task DeleteRestaurant(Restaurant restaurant)
    {
        var result = await DialogService.ShowMessageBox("Delete Restaurant", "Are you sure you want to delete this restaurant?", yesText: "Yes", cancelText: "Cancel");

        if (result == true)
        {
            if (restaurant != null)
            {
                Context.Restaurants.Remove(restaurant);
                await Context.SaveChangesAsync();
                await LoadRestaurants();
                Snackbar.Add("Restaurant deleted successfully");
            }
        }
    }
    
    private async Task OpenEditDialog(Restaurant restaurant)
    {
        var parameters = new DialogParameters();
        parameters.Add("Restaurant", restaurant);

        var dialog = DialogService.Show<TravelRestaurantEdit>("Edit Restaurant", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await LoadRestaurants();
            Snackbar.Add("Restaurant updated successfully");
        }
    }
}