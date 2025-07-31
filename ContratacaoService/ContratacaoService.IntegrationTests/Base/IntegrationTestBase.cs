using ContratacaoService.API;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ContratacaoService.IntegrationTests.Base
{
    public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient HttpClient;
        protected readonly WebApplicationFactory<Program> Factory;

        public IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            Factory = factory;

            HttpClient = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:7037")
            });
        }
    }
}