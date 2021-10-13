using Digitteck.Gateway.Service;
using NUnit.Framework;

namespace ConfigurationProviderTests
{
    public class JsonConfigurationProviderTests
    {
        private JsonConfigurationProvider provider;

        [SetUp]
        public void Setup()
        {
            //IContentProvider contentProvider = new ContentProvider();
            //this.provider = new JsonConfigurationProvider("JsonProviderTests/gwconf.json", contentProvider);
        }

        //[Test]
        public void json_configuration_provider_should_work()
        {
            GatewayConfiguration configurationModel = this.provider.Build();

            Assert.IsNotNull(configurationModel);
            Assert.IsNotNull(configurationModel.GlobalDirectives);
            Assert.IsNotNull(configurationModel.RouteModels);
        }
    }
}