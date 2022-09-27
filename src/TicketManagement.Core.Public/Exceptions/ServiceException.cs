using System.Runtime.Serialization;

namespace TicketManagement.Core.Public.Exceptions
{
    /// <summary>
    /// Exception throws when errors occurs in services work.
    /// </summary>
    [Serializable]
    public class ServiceException : Exception
    {
        public ServiceException()
            : base()
        {
        }

        public ServiceException(string message)
            : base(message)
        {
        }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}