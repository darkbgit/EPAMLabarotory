using TicketManagement.Core.Public.Clients;

namespace TicketManagement.EventManagement.API.Helpers
{
    internal class CustomOnTokenValidated
    {
        private readonly IAuthClient _authClient;

        public CustomOnTokenValidated(IAuthClient authClient)
        {
            _authClient = authClient;
        }
    }
}