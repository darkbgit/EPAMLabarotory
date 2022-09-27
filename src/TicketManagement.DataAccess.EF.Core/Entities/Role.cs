using Microsoft.AspNetCore.Identity;

namespace TicketManagement.DataAccess.EF.Core.Entities
{
    public sealed class Role : IdentityRole<Guid>
    {
        public Role(string name)
        : base(name)
        {
        }
    }
}