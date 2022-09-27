using System.Runtime.Serialization;

namespace TicketManagement.Core.Public.Exceptions
{
    /// <summary>
    /// Base exception class. Exceptions throws when entity not found.
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}