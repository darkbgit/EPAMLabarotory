// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Events => "Events";

        public static string EventsLoad => "EventsLoad";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EventslNavClass(ViewContext viewContext) => PageNavClass(viewContext, Events);

        public static string EventsLoadlNavClass(ViewContext viewContext) => PageNavClass(viewContext, EventsLoad);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
