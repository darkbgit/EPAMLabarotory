using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.UserManagement.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);

        string GetClaim(string token, string claimType);

        bool ValidateToken(string token);
    }
}
