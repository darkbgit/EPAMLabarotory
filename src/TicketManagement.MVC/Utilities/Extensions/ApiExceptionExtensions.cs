using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;

namespace TicketManagement.Core.Public.Extensions
{
    public static class ApiExceptionExtensions
    {
        public static async void ErrorsToModelStateErrors(this ApiException exception, PageModel pageModel)
        {
            var content = await exception.GetContentAsAsync<Dictionary<string, string>>();
            if (content != null)
            {
                foreach (var error in content)
                {
                    pageModel.ModelState.AddModelError(error.Key, error.Value);
                }
            }
        }
    }
}
