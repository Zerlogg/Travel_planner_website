using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TravelingTrips.Models;

namespace TravelingTrips.Pages;

public partial class ProfileDialog
{
    User user;
    string currentPassword;
    string newPassword;
    string confirmPassword;
    bool isShow;
    InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var currentUser = await userService.GetCurrentUser();
        if (currentUser != null)
        {
            user = new User
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                PasswordHash = currentUser.PasswordHash
            };
        }
    }

    void ShowPassword()
    {
        if (isShow)
        {
            isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else {
            isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
    
    private void ReloadPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }
    
    private async Task LogoutAndRedirect()
    {
        await JSRuntime.InvokeVoidAsync("window.location.assign", "/logout");
    }

    async Task ChangePassword()
    {
        if (user != null && 
            !string.IsNullOrWhiteSpace(currentPassword) && 
            !string.IsNullOrWhiteSpace(newPassword) && 
            !string.IsNullOrWhiteSpace(confirmPassword))
        {
            var isCurrentPasswordValid = await userService.VerifyUserPassword(user, currentPassword);
            if (!isCurrentPasswordValid)
            {
                await DialogService.ShowMessageBox("Error", "Incorrect current password.");
                return;
            }
            
            if (newPassword != confirmPassword)
            {
                await DialogService.ShowMessageBox("Error", "New password and confirmation password do not match.");
                return;
            }

            try
            {
                var passwordChanged = await userService.ChangeUserPassword(user.Id, currentPassword, newPassword);

                if (passwordChanged)
                {
                    await DialogService.ShowMessageBox("Success", "Password changed successfully.");
                }
                else
                {
                    await DialogService.ShowMessageBox("Error", "Failed to change password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing password: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                await DialogService.ShowMessageBox("Error", "An error occurred while changing the password. Please try again.");
            }
        }
        else
        {
            await DialogService.ShowMessageBox("Error", "Please fill out all required fields.");
        }
        
    }

    async Task Submit()
    {
        if (user != null)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;

                try
                {
                    await _context.SaveChangesAsync();
                    MudDialog.Close(DialogResult.Ok(true));
                    ReloadPage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving user: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"User with ID {user.Id} not found in the database.");
            }
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private async Task DeleteAccount()
    {
        var result = await DialogService.ShowMessageBox("Delete Account", "Are you sure you want to delete your account?", yesText: "Yes", cancelText: "Cancel");

        if (result == true)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser != null)
            {
                try
                {
                    _context.Users.Remove(existingUser);
                    await _context.SaveChangesAsync();
                    await LogoutAndRedirect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting user: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"User not found.");
            }
        }
    }
}