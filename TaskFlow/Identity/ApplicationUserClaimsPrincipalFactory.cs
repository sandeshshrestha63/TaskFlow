using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Identity
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly AppDbContext _context;
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,IOptions<IdentityOptions> optionsAccessor,AppDbContext context): base(userManager, optionsAccessor)
        {
            _context = context;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(CustomClaims.FullName, user.FullName ?? ""));

            if (user.CompanyId.HasValue)
            {
                identity.AddClaim(new Claim(CustomClaims.CompanyId,user.CompanyId.Value.ToString()));
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);

            if (employee != null)
            {
                identity.AddClaim(new Claim(CustomClaims.EmployeeId,employee.Id.ToString()));
            }
            var roles = await UserManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            return identity;
        }
    }
}
