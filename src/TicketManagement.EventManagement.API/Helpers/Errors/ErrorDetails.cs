using System.Text.Json;

namespace TicketManagement.EventManagement.API.Helpers.Errors
{
    internal class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}