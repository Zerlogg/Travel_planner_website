
using Microsoft.AspNetCore.Components.Authorization;

namespace TravelingTrips.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider _stateProvider;

        public UserService(AuthenticationStateProvider stateProvider)
        {
            _stateProvider = stateProvider;
        }
        public async Task<string> GetUserId()
        {
            var user = (await _stateProvider.GetAuthenticationStateAsync()).User;
            var UserId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
            return UserId;
        }
    }
}
