using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using PropostaService.API;

namespace PropostaService.IntegrationTests.Support
{
    public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient Client;

        protected IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            Client = factory.CreateClient();
        }
    }
}