using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using NUnit.Framework;
using TicketManagement.IntegrationTests.Helpers;

namespace TicketManagement.IntegrationTests.WebApp.Pages
{
    internal class EventsLoadPageTest : BaseTest
    {
        [Test]
        public async Task Post_UploadFileGivenNoData_ReturnsPage()
        {
            // Arrange
            var client = GetClient();

            var defaultPage = await client.GetAsync("/Moderator/Manage/EventsLoad");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var response = await client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form"),
                (IHtmlInputElement)content.QuerySelector("input[id='uploadButton']"));

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, defaultPage.StatusCode);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}