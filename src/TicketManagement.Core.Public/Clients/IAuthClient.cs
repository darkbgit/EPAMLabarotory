using Refit;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.Public.Responses.UserManagement;

namespace TicketManagement.Core.Public.Clients;

public interface IAuthClient
{
    [Post("/users/user-info")]
    Task<UserInfoResponse> GetUserInfo(TokenRequest request);

    [Post("/auth/validate-token")]
    Task<bool> ValidateToken(TokenRequest request);

    [Post("/users/{id}/get-roles")]
    Task<Roles> GetUserRole(Guid id);
}