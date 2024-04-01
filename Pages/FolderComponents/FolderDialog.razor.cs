using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.FolderComponents;

public partial class FolderDialog
{
    private bool _success;
    
    private string[] _errors = Array.Empty<string>();

    private MudForm? _form;

    
    [Parameter]
    public int? Id { get; set; }

    [SupplyParameterFromQuery]
    private Folder CurrentFolder { get; set; } = new();

    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
    
    private void ReloadPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

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
        ReloadPage();
    }
    
    async Task<string> getUserId(){
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var UserId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
        return UserId;
    }

    private async Task CreateFolder()
    {
        CurrentFolder.UserId = await getUserId();
        Context.Folders.Add(CurrentFolder);
        
        await Context.SaveChangesAsync();

        MudDialog?.Close(DialogResult.Ok(true));
        
        Snackbar.Add("Folder was successfully created");
        ReloadPage();
    }

    private void Cancel() => MudDialog?.Cancel();
}