using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TravelingTrips.Areas.Identity.Pages.Account;

public class Register : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public Register(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
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
            var existingUsername = await _userManager.FindByNameAsync(Input.Username);
            if (existingUsername != null)
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return Page();
            }
            
            var existingEmail = await _userManager.FindByEmailAsync(Input.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError(string.Empty, "Email already exists.");
                return Page();
            }
            
            if (Input.Password.Length < 5)
            {
                ModelState.AddModelError(string.Empty, "Password must be at least 5 characters long.");
                return Page();
            }
            
            var identity = new IdentityUser { UserName = Input.Username, Email = Input.Email };
            var result = await _userManager.CreateAsync(identity, Input.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(identity, isPersistent: false);
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }
        
        return Page();
    }

    public class InputModel
    {
        [Required]
        [StringLength(16, MinimumLength = 1)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}