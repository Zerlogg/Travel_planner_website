using Microsoft.AspNetCore.Components;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.FolderComponents;

public partial class FolderDialog
{
    [Parameter]
    public int? Id { get; set; }

    [SupplyParameterFromQuery]
    private Folder CurrentFolder { get; set; } = new();

    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (Id != null)
        {
            var folder = await Context.Folders.FindAsync(Id);

            if (folder != null)
            {
                CurrentFolder.Title ??= folder.Title;
            }
        }
    }

    private async Task HandleSubmit()
    {
        if (Id != null)
        {
            await EditFolder();
        }
        else
        {
            await CreateFolder();
        }
    }

    private async Task EditFolder()
    {
        var dbFolder = await Context.Folders.FindAsync(Id);
        
        if (dbFolder != null)
        {
            dbFolder.Title = CurrentFolder.Title;

            await Context.SaveChangesAsync();
            
            StateHasChanged();
            
            Snackbar.Add("Folder was successfully saved");
        }

        MudDialog?.Close(DialogResult.Ok(true));
    }

    private async Task CreateFolder()
    {
        Context.Folders.Add(CurrentFolder);
        
        await Context.SaveChangesAsync();

        MudDialog?.Close(DialogResult.Ok(true));
        
        Snackbar.Add("Folder was successfully created");
    }

    private void Cancel() => MudDialog?.Cancel();
}