using AutoMapper;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.UserManagement.Services.Mapping
{
    internal class UserManagementMappingProfile : Profile
    {
        public UserManagementMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<UserForCreateDto, User>();

            CreateMap<UserForUpdateRequest, User>();

            CreateMap<User, UserWithRolesDto>()
                .ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}
