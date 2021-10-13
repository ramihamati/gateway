using Digitteck.Gateway.Service.Exceptions;

namespace Digitteck.Gateway.Service
{
    public class ContentProviderBuilder
    {
        private string _GlobalConfFileName = "";

        private IContentProvider _ContentProvider;

        public ContentProviderBuilder UseGlobalConfiguration(string globalConfigFileName)
        {
            this._GlobalConfFileName = globalConfigFileName;
            return this;
        }

        public ContentProviderBuilder UseJsonContentProvider()
        {
            if (string.IsNullOrWhiteSpace(_GlobalConfFileName))
            {
                throw new GatewayException(ErrorCode.ConfigurationProvider, "Must pass a name for the global configuration file before using the JsonContentProvider");
            }

            var jsonContentProvider = new JsonContentProvider(_GlobalConfFileName);
            jsonContentProvider.Build();
            this._ContentProvider = jsonContentProvider;

            return this;
        }

        internal IContentProvider GetContentProvider()
        {
            if (_ContentProvider == null)
            {
                throw new GatewayException(ErrorCode.ContentProvider, "A content provider has not been chosed. Use services.UseContentProvider");
            }

            return _ContentProvider;
        }
    }
}