using Refit;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.Public.Responses.UserManagement;

namespace TicketManagement.MVC.Clients.UserManagement
{
    public interface IUsersClient
    {
        [Post("/auth/login")]
        Task<string> Login(LoginRequest request);

        [Post("/users/user-info")]
        Task<UserInfoResponse> GetUserInfo(TokenRequest request);

        [Get("/users/user")]
        Task<UserDto> GetUser();

        [Get("/users/paginated-with-roles")]
        Task<PaginatedList<UserWithRolesDto>> GetPaginatedUsers(PaginationRequest request);

        [Post("/users/register")]
        Task<UserDto> CreateUser([Body] UserForCreateDto dto);

        [Put("/users/{id}")]
        Task UpdateUser(Guid id, [Body] UserForUpdateRequest dto);

        [Post("/users/{id}/add-to-role")]
        Task AddToRole(Guid id, [Body] AddToRoleRequest request);

        [Delete("/users/{id}")]
        Task DeleteUser(Guid id);

        [Get("/users/{id}/balance")]
        Task<decimal> GetUserBalance(Guid id);

        [Patch("/users/{id}/balance")]
        Task TopUpUserBalance(Guid id, TopUpBalanceRequest request);

        [Post("/users/{id}")]
        Task ChangePasword(Guid id, ChangePasswordRequest request);
    }
}
