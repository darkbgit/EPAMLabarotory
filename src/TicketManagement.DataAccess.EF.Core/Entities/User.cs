using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TicketManagement.DataAccess.EF.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string TimeZoneId { get; set; }
        public string Language { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        [Precision(18, 0)]
        public decimal Balance { get; set; }
    }
}