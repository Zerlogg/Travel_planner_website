using TravelingTrips.Models;

namespace TravelingTrips.Services;

public interface IUserService
{
    Task UpdateUser(User user);
    Task<User> GetCurrentUser();
    Task<string> GetUserId(); 
    Task<bool> VerifyUserPassword(User user, string password);
    Task<bool> ChangeUserPassword(string userId, string currentPassword, string newPassword);
}
