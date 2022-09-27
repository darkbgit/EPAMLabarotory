using System.Threading.Tasks;
using NUnit.Framework;

namespace TicketManagement.IntegrationTests.WebApp
{
    [TestFixture]
    internal class BasicTests : BaseTest
    {
        [Test]
        [TestCase("/Home/Index")]
        [TestCase("/Home/Privacy")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = GetClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8",
                response.Content.Headers.ContentType?.ToString());
        }
    }
}