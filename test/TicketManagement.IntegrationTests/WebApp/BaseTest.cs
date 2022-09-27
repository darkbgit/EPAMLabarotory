using System;
using System.Net.Http;

namespace TicketManagement.IntegrationTests.WebApp
{
    internal class BaseTest : IDisposable
    {
        private readonly CustomWebApplicationFactory<MVC.Program> _webApplicationFactory;

        protected BaseTest()
        {
            _webApplicationFactory = new CustomWebApplicationFactory<MVC.Program>();
        }

        public HttpClient GetClient() => _webApplicationFactory.CreateClient();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _webApplicationFactory.Dispose();
        }
    }
}