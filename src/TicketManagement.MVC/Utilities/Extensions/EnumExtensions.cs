using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TicketManagement.MVC.Utilities.Extensions
{
    internal static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetDescription();
        }
    }
}