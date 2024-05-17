using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageRestaurant;

public partial class TravelRestaurantAdd
{
    private Restaurant restaurant = new Restaurant();
    private IBrowserFile uploadedImage;
    private bool _success;
    private string[] _errors = Array.Empty<string>();
    private MudForm? _form;
    public bool IsSelected { get; set; } = false;

    [Parameter]
    public Travel CurrentTrip { get; set; } 
    
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    private void Cancel() => MudDialog?.Cancel();

    private async Task SaveRestaurant()
    {
        if (CurrentTrip != null)
        {
            byte[] imageBytes;

            if (uploadedImage == null)
            {
                using (var defaultImageStream = new FileStream("wwwroot/Images/Restaurant.PNG", FileMode.Open))
                {
                    imageBytes = new byte[defaultImageStream.Length];
                    await defaultImageStream.ReadAsync(imageBytes);
                }
            }
            else
            {
                if (uploadedImage.Size > 512000)
                {
                    Snackbar.Add("File size exceeds the maximum allowed size (512 KB). Please choose a smaller file.", Severity.Warning);
                    return;
                }

                imageBytes = new byte[uploadedImage.Size];
                await uploadedImage.OpenReadStream().ReadAsync(imageBytes);
            }

            restaurant.TravelId = CurrentTrip.Id;
            restaurant.Image = imageBytes;
            restaurant.IsSelected = false;

            Context.Restaurants.Add(restaurant);
            await Context.SaveChangesAsync();

            Snackbar.Add("Restaurant added successfully");
            MudDialog?.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add("Please fill in all required fields.", Severity.Error);
        }
    }
    
    private void UploadPicture(IBrowserFile file)
    {
        uploadedImage = file;
    }
}