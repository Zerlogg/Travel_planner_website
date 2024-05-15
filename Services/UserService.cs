using Microsoft.AspNetCore.Identity;
using TravelingTrips.Data;
using TravelingTrips.Models;

namespace TravelingTrips.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<string> GetUserId()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return user?.Id;
        }

        public async Task<User> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }

        public async Task UpdateUser(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> VerifyUserPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> ChangeUserPassword(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return false;
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error changing password: {error.Description}");
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception changing password: {ex.Message}");
                return false;
            }
        }
    }
}
