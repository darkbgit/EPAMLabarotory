using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.Public.Responses.UserManagement;
using TicketManagement.Core.UserManagement.Services.Interfaces;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.UserManagement.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IStringLocalizer<UsersService> _stringLocalizer;

        public UsersService(UserManager<User> userManager,
            IJwtService jwtService,
            IMapper mapper,
            IStringLocalizer<UsersService> stringLocalizer)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
                if (user == null)
                {
                    throw new ServiceException(_stringLocalizer["error.UserNotFound"]);
                }
            }

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isValid)
            {
                throw new ServiceException("Could not authenticate user.");
            }

            var token = _jwtService.GenerateToken(user);

            return token;
        }

        public async Task<UserInfoResponse?> GetUserInfoAsync(string token)
        {
            var id = _jwtService.GetClaim(token, JwtRegisteredClaimNames.NameId);

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(guidId.ToString());

            if (user == null)
            {
                return null;
            }

            var result = new UserInfoResponse
            {
                Id = user.Id,
                Locale = user.Language,
                TimeZoneId = user.TimeZoneId,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
            };
            return result;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            var result = _mapper.Map<UserDto>(user);

            return result;
        }

        public async Task<Guid> CreateUserAsync(UserForCreateDto dto)
        {
            var isExist = await _userManager.Users
                .AnyAsync(user => user.UserName == dto.UserName || user.Email == dto.Email);

            if (isExist)
            {
                throw new ServiceException("User with same name or email is exists.");
            }

            var user = _mapper.Map<User>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var error = "Couldn't create user. try again.";

                if (result.Errors.Any())
                {
                    error = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description).ToList());
                }

                throw new ServiceException(error);
            }

            return user.Id;
        }

        public async Task AddToRoleAsync(Guid userId, Roles role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ServiceException("User not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.ToString());

            if (!result.Succeeded)
            {
                throw new ServiceException("Couldn't create user. Try again.");
            }
        }

        public async Task UpdateUserAsync(UserForUpdateRequest dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());

            if (user == null)
            {
                throw new ServiceException("Couldn't update user. Try again.");
            }

            _mapper.Map(dto, user);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors);
                throw new ServiceException($"Couldn't update user. {errors}");
            }
        }

        public async Task<PaginatedList<UserWithRolesDto>> GetPagedUsersWithRolesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var users = await _userManager.Users
                .OrderBy(user => user.Email)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var usersWithRoles = users
                .Select(user =>
                {
                    var dto = _mapper.Map<UserWithRolesDto>(user);
                    dto.Roles = _userManager.GetRolesAsync(user).Result.ToList();
                    return dto;
                })
                .ToList();

            var count = await _userManager.Users.CountAsync(cancellationToken);

            var result = new PaginatedList<UserWithRolesDto>(usersWithRoles,
                count,
                pageNumber,
                pageSize);

            return result;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ServiceException("User doesn't exist.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors);
                throw new ServiceException($"Couldn't delete user. {errors}");
            }
        }

        public async Task<decimal> GetUserBalance(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new ServiceException("User doesn't exist.");
            }

            return user.Balance;
        }

        public async Task TopUpBalance(Guid id, decimal additionalBalance)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new ServiceException("User doesn't exist.");
            }

            user.Balance += additionalBalance;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors);
                throw new ServiceException($"Couldn't top up user balance. {errors}");
            }
        }
    }
}