using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;
using TravelingTrips.Pages.TravelComponents;
using TravelingTrips.Pages.TravelComponents.Print;

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
    
    private void ReloadPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }
    
    private async Task LoadFolders()
    {
        var userId = await getUserId();
        _folderList = await Context.Folders.Where(x => x.UserId == userId).ToListAsync();
        foreach (var item in _folderList)
        {
            item.Travels = await Context.Travels.Join(Context.TravelFolders.Where(x => x.FolderId == item.Id && x.IsEnabled == true),
                    t => t.Id,
                    tf => tf.TravelId,
                    (t, tf) => t)
                .ToListAsync();
        }
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
        var result = await DialogService.ShowMessageBox("Delete Collection", "Are you sure you want to delete this collection?", yesText: "Yes", cancelText: "Cancel");

        if (result == true)
        {
            var dbFolder = await Context.Folders.FindAsync(id);
            if (dbFolder != null)
            {
                Context.Remove(dbFolder);
                await Context.SaveChangesAsync();
                Snackbar.Add("Folder was successfully deleted");
            }

            ReloadPage();
        }
    }
    
    private void AddFolder()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        DialogService.Show<FolderDialog>("Add collection", options);
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
    
    private void NavigateToDetails(int id)
    {
        NavigationManager.NavigateTo($"travelpage/{id}");
    }
    
    private void DownloadFile(string filename)
    {
        var pdf = new PDFGenerator();
        pdf.DownloadPDF(js, filename);
    }
    
    private void EditTravel(int id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var options = new DialogOptions { CloseOnEscapeKey = true };
        DialogService.Show<TravelDialog>("Edit trip", parameters, options);
    }
    
    private async Task DeleteTravel(int id)
    {
        var result = await DialogService.ShowMessageBox("Delete Trip", "Are you sure you want to delete this trip?", yesText: "Yes", cancelText: "Cancel");

        if (result == true)
        {
            var dbTravel = await Context.Travels.FindAsync(id);
            if (dbTravel != null)
            {
                Context.Remove(dbTravel);
                await Context.SaveChangesAsync();
                Snackbar.Add("Trip was successfully deleted");
            }

            ReloadPage();
        }
    }
    
    private string GetTextColorForBackground(string? backgroundColor)
    {
        if (string.IsNullOrWhiteSpace(backgroundColor))
        {
            return "black";
        }
        
        var color = System.Drawing.ColorTranslator.FromHtml(backgroundColor);
        
        double luminance = 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B;
        
        return luminance > 128 ? "black" : "white";
    }
}