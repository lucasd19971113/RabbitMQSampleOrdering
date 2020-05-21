using RabbitMQOrdering.Integration.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net; 
using System.Threading.Tasks;
using Xunit;

namespace RabbitMQOrdering.Integration.Tests.Scenarios
{
    public class OrderTest : TestContext
    {
        private readonly WebApplicationFactory<RabbitMQOrdering.Api.Startup> _factory;

        public OrderTest(WebApplicationFactory<RabbitMQOrdering.Api.Startup> factory) : base(factory)
        {
        
        }

        [Fact]
        public async Task Order_Get_ReturnsOkResponse()
        {
            var response = await base.Client.GetAsync("/api/Order/getall");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

  

        [Fact]
        public async Task Order_GetById_ReturnsNotFoundResponse()
        {
            var response = await base.Client.GetAsync("/api/Order/Get/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        
    }
}