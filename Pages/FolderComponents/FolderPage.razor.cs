using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.FolderComponents;

public partial class FolderPage
{
    private string? TextValue { get; set; }

    private List<Folder> _folderList = new List<Folder>();
    
    private List<Folder> _filteredFolders = new List<Folder>();
        
    protected override async Task OnInitializedAsync()
    {
        await LoadFolders();
    }
    
    private async Task LoadFolders()
    {
        var userId = await getUserId();
        _folderList = await Context.Folders.Where(x => x.UserId == userId).ToListAsync();
        ApplySearchFilter();
    }
    
    async Task<string> getUserId(){
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var UserId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
        return UserId;
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
    
    private void ApplySearchFilter()
    {
        if (!string.IsNullOrWhiteSpace(TextValue))
        {
            _filteredFolders = _folderList.Where(f => f.Title != null && f.Title.Contains(TextValue, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        else
        {
            _filteredFolders = new List<Folder>(_folderList);
        }
    }
    
    private void SearchFolders()
    {
        ApplySearchFilter();
        StateHasChanged();
    }
}