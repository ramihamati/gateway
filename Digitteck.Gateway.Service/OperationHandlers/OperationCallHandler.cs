using Digitteck.Gateway.Service.Common;
using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.Service.SpecializedModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public sealed class OperationCallHandler : OperationHandlerCore<OperationCall>
    {
        public OperationCall Operation { get; private set; }

        private HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (mLock)
                    {
                        if (_client == null)
                        {
                            _client = _httpClientProvider.GetClient();
                        }
                    }
                }

                return _client;
            }
        }

        private readonly IPlaceholderExtractor _placeholderExtractor;
        private readonly IUriHelperService _uriHelperService;
        private readonly IRequestMapper _requestMapper;
        private readonly IHttpClientProvider _httpClientProvider;
        private static HttpClient _client;
        private static object mLock = new object();
        /// <summary>
        /// The constructor can be used for parameter injection
        /// </summary>
        public OperationCallHandler(IPlaceholderExtractor placeholderExtractor,
            IUriHelperService uriHelperService,
            IRequestMapper requestMapper,
            IHttpClientProvider httpClientProvider)
        {
            _placeholderExtractor = placeholderExtractor;
            _uriHelperService = uriHelperService;
            _requestMapper = requestMapper;
            _httpClientProvider = httpClientProvider;
        }

        public override void Initialize(OperationCall operation)
        {
            this.Operation = operation;
        }

        //Notes in code made with the sample : 
        //Upstream Template : /movies/{movieName}?Rating={rating}
        //Request : /movies/{movieName}?Rating=9.7&Year=2019
        //mathing is made against the path and these will match
        //placeholders will be replaces (movieName and Rating)
        //the rest of query parst (Year) will be added
        public override async Task<OperationResponse> Execute(GatewayContext context, RouteContext routeContext)
        {
            try
            {
                //Handle income data
                //path with query : "/api/Movies?Rating=9.7"
                TemplatePathAndQueryObject upstreamTemplatePathAndQuery = routeContext.UpstreamTemplate.EntryTemplate;
                PathAndQueryObject requestPathAndQuery = _uriHelperService.GetFullRelativePath(context.HttpContext.Request);

                ValidateUpstreamDownstreamPlaceholderDefinitions(routeContext);
                /*  get values from the request
                 *  the placeholder values according to the incoming request(getting the routevaluedictionary and query data from
                 *  the HttpRequest is not valid because they might not share the same names as defined in the template)
                 */

                List<PlaceholderValue> requestPlaceholdersValues = _placeholderExtractor.ExtractPlaceholderValuesFromQuery(upstreamTemplatePathAndQuery, requestPathAndQuery);

                //replacing placehoders in the path
                PathObject downstreamPathWithNoPlaceholders = _placeholderExtractor.ReplacePlaceholdersWithValues(this.Operation.PathAndQuery.GetPathObject(), requestPlaceholdersValues);
                QueryObject downstreamQueryWithNoPlaceholders = _placeholderExtractor.ReplacePlaceholdersWithValues(this.Operation.PathAndQuery.GetQueryObject(), requestPlaceholdersValues);

                //the downstreamQuery represents the query from the template (if defined) with placeholder values(if any) filled in based on the request
                //check if there are any placeholders that were not replaces

                if (_placeholderExtractor.HasPlaceholders(downstreamPathWithNoPlaceholders.Value)
                    || _placeholderExtractor.HasPlaceholders(downstreamQueryWithNoPlaceholders.Value))
                {
                    throw new Exception($"The request {requestPathAndQuery.Value} could fill all placeholder values in {this.Operation.PathAndQuery.Value}");
                }

                //get the query from the request that was not mapped in the template
                downstreamQueryWithNoPlaceholders = _uriHelperService.Union(downstreamQueryWithNoPlaceholders, requestPathAndQuery.GetQueryObject());

                //create the absolute uri
                AbsoluteUriObject absoluteUri = AbsoluteUriObject.Create(
                        this.Operation.Scheme,
                        this.Operation.ServerHost,
                        this.Operation.ServerPort,
                        downstreamPathWithNoPlaceholders,
                        downstreamQueryWithNoPlaceholders
                    );

                HttpRequestMessage mapResponse = await _requestMapper.MapAsync(context.HttpContext.Request, this.Operation.HttpMethod, absoluteUri);

                OperationByteArrayResponse apiResponse = new OperationByteArrayResponse(new byte[] { }, System.Net.HttpStatusCode.NoContent);


                using (mapResponse)
                using (var response = await Client.SendAsync(mapResponse).ConfigureAwait(false))
                {
                    apiResponse = await OperationByteArrayResponse.FromAsync(response).ConfigureAwait(false);
                }

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new OperationStringReponse(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private void ValidateUpstreamDownstreamPlaceholderDefinitions(RouteContext routeContext)
        {
            //the placeholders according to the template defined in the json document
            List<Placeholder> upstreamPlaceholders = routeContext.UpstreamTemplate.TemplatePlaceholders;

            //get placeholders for downstream calls
            List<Placeholder> downstreamPlaceholders = _placeholderExtractor.GetPlaceholdersFromTemplate(this.Operation.PathAndQuery);

            //validate placeholders. The upstream should pe able to suply all placeholders needed by the downstream

            if (!_placeholderExtractor.IsProperSuperset(upstreamPlaceholders.ToArray(), downstreamPlaceholders.ToArray()))
            {
                throw new System.Exception($"Downstream contains placeholders that are not defined in the upstream");
            }
        }
    }
}
