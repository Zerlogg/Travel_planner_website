using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageDays.TravelDaysObjects;

public partial class TravelTourismObjectsAdd : ComponentBase
{
    [Parameter]
    public int TravelDayId { get; set; }
    
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    private TourismObject tourismObject = new();
    private IBrowserFile uploadedImage;
    private bool _success;
    private string[] _errors = Array.Empty<string>();
    private MudForm? _form;

    private void Cancel() => MudDialog?.Cancel();

    private async Task SaveTourismObject()
    {
        if (_form == null) return;
        
        await _form.Validate();

        if (_success)
        {
            byte[] imageBytes;

            if (uploadedImage == null)
            {
                using (var defaultImageStream = new FileStream("wwwroot/Images/TourismObject.jpg", FileMode.Open))
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

            tourismObject.TravelDayId = TravelDayId.ToString();
            tourismObject.Image = imageBytes;

            await using var context = ContextFactory.CreateDbContext();
            context.TourismObjects.Add(tourismObject);
            await context.SaveChangesAsync();

            Snackbar.Add("Tourism object added successfully");
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