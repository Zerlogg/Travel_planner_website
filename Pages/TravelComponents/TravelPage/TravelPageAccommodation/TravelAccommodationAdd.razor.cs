using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageAccommodation;

public partial class TravelAccommodationAdd
{
    private Accommodation accommodation = new Accommodation();
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

    private async Task SaveAccommodation()
    {
        if (CurrentTrip != null)
        {
            byte[] imageBytes;

            if (uploadedImage == null)
            {
                using (var defaultImageStream = new FileStream("wwwroot/Images/Hotel.PNG", FileMode.Open))
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

            accommodation.TravelId = CurrentTrip.Id.ToString();
            accommodation.Image = imageBytes;
            accommodation.IsSelected = false;

            Context.Accommodations.Add(accommodation);
            await Context.SaveChangesAsync();

            Snackbar.Add("Accommodation added successfully");
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