﻿namespace TicketManagement.Core.Public.DTOs.UserDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string TimeZoneId { get; set; }
        public string Language { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public decimal Balance { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
