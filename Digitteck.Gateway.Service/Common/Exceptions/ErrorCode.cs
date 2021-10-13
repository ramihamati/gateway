namespace Digitteck.Gateway.Service
{
    public enum ErrorCode
    {
        NotCategorized = 0,
        UnmapableRequest = 1,
        UnmappableMethodType = 2,
        MappingError = 3,
        JSConverterBuilder = 4,
        ConfigurationProvider = 5,
        ContentProvider = 6,
        ObjectFactory = 7,
        OperationDispatchUnit = 8,
        OperationGroupDispatcher = 9,
        OperationHandlersStore = 10,
        OperationResponseConverter = 11,
        OperationResultStore = 12,
        PlaceholderExtractor = 13,
        RouteMatching = 14,
        OperationReturnHandler = 15,
        OperationAggregateResponseHandler = 16,
        OperationByteArrayResponse = 17,
        FileHelper = 18,
        GWRuntime = 19
    }
}
