using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageRestaurant;

public partial class TravelRestaurantEdit
{
    [Parameter]
    public Restaurant restaurant { get; set; }
    
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    private IBrowserFile uploadedImage;
    private bool _success;
    private string[] _errors = Array.Empty<string>();
    private MudForm? _form;
    
    private void Cancel()
    {
        MudDialog?.Cancel();
    }

    private async Task SaveRestaurant()
    {
        if (restaurant != null)
        {
            if (uploadedImage != null)
            {
                if (uploadedImage.Size > 512000)
                {
                    Snackbar.Add("File size exceeds the maximum allowed size (512 KB). Please choose a smaller file.", Severity.Warning);
                    return;
                }

                var imageBytes = new byte[uploadedImage.Size];
                await uploadedImage.OpenReadStream().ReadAsync(imageBytes);

                restaurant.Image = imageBytes;
            }
            
            Context.Restaurants.Update(restaurant);
            await Context.SaveChangesAsync();

            Snackbar.Add("Restaurant updated successfully");
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