using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace RabbitMQOrdering.Integration.Tests.Fixtures
{
    public class TestContext : IClassFixture<WebApplicationFactory<RabbitMQOrdering.Api.Startup>>
    {
        public HttpClient Client { get; set; }

        private readonly WebApplicationFactory<RabbitMQOrdering.Api.Startup> _factory;
        
        public TestContext (WebApplicationFactory<RabbitMQOrdering.Api.Startup> factory) 
        {
            this._factory = factory;
            SetupClient();           
        }
        public void SetupClient () {
            Client = _factory.CreateClient();
        }
    }
}