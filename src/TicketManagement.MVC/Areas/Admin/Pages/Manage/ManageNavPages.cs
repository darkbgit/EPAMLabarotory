﻿#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketManagement.MVC.Areas.Admin.Pages.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string AdminPanel => "AdminPanel";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string AdminPanelNavClass(ViewContext viewContext) => PageNavClass(viewContext, AdminPanel);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
