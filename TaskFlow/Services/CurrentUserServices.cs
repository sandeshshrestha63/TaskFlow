using Microsoft.AspNetCore.Identity;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class CurrentUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserServices(UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> GetCompanyId()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            return user?.CompanyId?? 0;
        }
    }
}
