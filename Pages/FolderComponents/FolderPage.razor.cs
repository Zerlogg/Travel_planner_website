using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.FolderComponents;

public partial class FolderPage
{
    private string? TextValue { get; set; }

    private List<Folder> _folderList = new List<Folder>();
        
    protected override async Task OnInitializedAsync()
    {
        _folderList = await Context.Folders.ToListAsync();
    }
    
    private void EditFolder(int id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var options = new DialogOptions { CloseOnEscapeKey = true };
        DialogService.Show<FolderDialog>("Edit folder", parameters, options);
    }

    private async Task DeleteFolder(int id)
    {
        var dbFolder = await Context.Folders.FindAsync(id);
        if (dbFolder != null)
        {
            Context.Remove(dbFolder);
            await Context.SaveChangesAsync();
            Snackbar.Add("Folder was successfully deleted");
        }
    }
    
    private void AddFolder()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        DialogService.Show<FolderDialog>("Add folder", options);
    }
}