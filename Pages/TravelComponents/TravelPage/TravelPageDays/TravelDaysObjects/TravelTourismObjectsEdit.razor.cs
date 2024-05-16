using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents.TravelPage.TravelPageDays.TravelDaysObjects;

public partial class TravelTourismObjectsEdit : ComponentBase
{
    [Parameter]
    public TourismObject tourismObject { get; set; }

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

    private async Task SaveTourismObject()
    {
        if (tourismObject != null)
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

                tourismObject.Image = imageBytes;
            }

            using var context = await ContextFactory.CreateDbContextAsync();
            context.TourismObjects.Update(tourismObject);
            await context.SaveChangesAsync();

            Snackbar.Add("Tourism Object updated successfully");
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