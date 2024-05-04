using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TravelingTrips.Areas.Identity.Pages.Account;

public class Login : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public Login(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    [BindProperty]
    public InputModel Input { get; set; }
    public string ReturnUrl { get; set; }
    
    public void OnGet()
    {
        ReturnUrl = Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ReturnUrl = Url.Content("~/home");

        if (ModelState.IsValid)
        {
            var userExists = await _signInManager.UserManager.FindByNameAsync(Input.Username);
        
            if (userExists == null)
            {
                ModelState.AddModelError(string.Empty, "User does not exist.");
                return Page();
            }
            
            var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(Input.Username);
                ViewData["UserId"] = user.Id;
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }
        
        return Page();
    }
    
    public class InputModel
    {
        [Required]
        [StringLength(16, MinimumLength = 3)]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}