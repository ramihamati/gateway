using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class RouteMatchingTests
    {
        //gate api
        private GatewayFactory webApplicationFactory;
        //movies api

        private HttpClient gateClient;

        [SetUp]
        public void Setup()
        {
            this.webApplicationFactory = new GatewayFactory();
            this.gateClient = this.webApplicationFactory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            this.gateClient?.Dispose();
        }

        [Test]
        public async Task SendRequest_1()
        {
            var client = this.webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/movies");
        }

        [Test]
        public async Task SendRequest_2()
        {
            try
            {
                var client = this.webApplicationFactory.CreateClient();

                var response = await client.GetAsync("/movies/AmericanGangster?Rating=9.7");

                string value = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
