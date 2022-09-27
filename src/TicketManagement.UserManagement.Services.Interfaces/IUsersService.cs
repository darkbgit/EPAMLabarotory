using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.Public.Responses.UserManagement;

namespace TicketManagement.Core.UserManagement.Services.Interfaces
{
    public interface IUsersService
    {
        Task<string> LoginAsync(LoginRequest request);

        Task<UserInfoResponse?> GetUserInfoAsync(string token);

        Task<UserDto?> GetUserByIdAsync(Guid id);

        Task<PaginatedList<UserWithRolesDto>> GetPagedUsersWithRolesAsync(int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<Guid> CreateUserAsync(UserForCreateDto dto);

        Task AddToRoleAsync(Guid userId, Roles role);

        Task UpdateUserAsync(UserForUpdateRequest dto);

        Task DeleteUserAsync(Guid userId);

        Task<decimal> GetUserBalance(Guid id);

        Task TopUpBalance(Guid id, decimal additionalBalance);
    }
}
