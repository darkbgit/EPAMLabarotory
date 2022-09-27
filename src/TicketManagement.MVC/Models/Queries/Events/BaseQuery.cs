using System.Reflection;

namespace TicketManagement.MVC.Models.Queries.Events
{
    public class BaseQuery
    {
        protected BaseQuery()
        {
            // q
        }

        public Dictionary<string, string> ToDictionary()
        {
            return GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(this, null)?.ToString());
        }
    }
}