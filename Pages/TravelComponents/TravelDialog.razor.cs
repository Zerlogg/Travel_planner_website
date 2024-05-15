using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages.TravelComponents;

public partial class TravelDialog
{
    private bool _success;
    
    private string[] _errors = Array.Empty<string>();

    private MudForm? _form;
    
    private IEnumerable<Folder> _folders = new List<Folder>();

    private List<Folder> checkedFolders { get; set; }
    private IEnumerable<Folder> options { get; set; }

    private string userId { get; set; }

    [Parameter]
    public int? Id { get; set; }

    [SupplyParameterFromQuery]
    private Travel CurrentTrip { get; set; } = new();

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }
    
    private void ReloadPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    protected override async Task OnInitializedAsync()
    {
        await GetFolders();
        var travelFolders = await Context.TravelFolders.Where(x => x.TravelId == Id && x.IsEnabled == true).ToListAsync();
        checkedFolders = _folders.Join(travelFolders, x => x.Id, y => y.FolderId, (x, y) => new Folder { Id = x.Id, IsEnabled = y.IsEnabled, Title = x.Title, UserId = x.UserId }).ToList();
        options = new HashSet<Folder>(checkedFolders);

    }
    
    async Task<string> getUserId(){
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var UserId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
        return UserId;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (Id != null)
        {
            var travel = await Context.Travels.FindAsync(Id);

            if (travel != null)
            {
                CurrentTrip.City ??= travel.City;
                CurrentTrip.Description ??= travel.Description;
                CurrentTrip.StartDate ??= travel.StartDate;
                CurrentTrip.EndDate ??= travel.EndDate;
                CurrentTrip.Budget ??= travel.Budget;
                CurrentTrip.Preferences ??= travel.Preferences;
            }
        }
    }

    private async Task GetFolders()
    {
        userId = await getUserId();
        _folders = await Context.Folders.Where(x => x.UserId == userId).ToListAsync();
    }

    private async Task EditTrip()
    {
        var dbTrip = await Context.Travels.FindAsync(Id);
        
        if (dbTrip != null)
        {
            dbTrip.City = CurrentTrip.City;
            dbTrip.Description = CurrentTrip.Description;
            dbTrip.Budget = CurrentTrip.Budget;
            dbTrip.Preferences = CurrentTrip.Preferences;

            foreach (var item in options)
            {
                var model = await Context.TravelFolders.Where(x => x.FolderId == item.Id && x.TravelId == dbTrip.Id).FirstOrDefaultAsync();
                
                if (model == null)
                {
                    var record = new TravelFolders()
                    {
                        FolderId = item.Id,
                        TravelId = dbTrip.Id,
                        IsEnabled = true,
                    };

                    Context.Add(record);
                }
                else
                {
                    model.IsEnabled = true;
                }
            }

            var except = checkedFolders.Except(options).ToList();
            foreach (var item in except)
            {
                var model = await Context.TravelFolders.Where(x => x.FolderId == item.Id && x.TravelId == dbTrip.Id).FirstOrDefaultAsync();
                if (model != null)
                    model.IsEnabled = false;
            }

            await Context.SaveChangesAsync();
            
            StateHasChanged();
            
            Snackbar.Add("Trip was successfully saved");
        }

        MudDialog?.Close(DialogResult.Ok(true));
        ReloadPage();
    }

    private void Cancel() => MudDialog?.Cancel();
}