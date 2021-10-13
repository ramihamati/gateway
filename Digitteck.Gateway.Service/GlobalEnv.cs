namespace Digitteck.Gateway.Service
{
    public static class GlobalEnv
    {
        /*
         * The uri URI = scheme:[//authority]path[?query][#fragment]
         */
        public const char QueryDelimiter = '?';
        public const char QueryVarDelimiter = '&';
        public const char QueryAssignment = '=';
        public const char PathDelimiter = '/';
        public const char InvalidPathDelimiter = '\\';
        public const char PortDelimiter = ':';
        public const string SchemeHttp = "http";
        public const string SchemeHttps = "https";
        public const string SchemeAndAuthorityDelimiter = "://";
        public const char PlaceholderStart = '{';
        public const char PlaceholderEnd = '}';

        //used in template to match everything to the end of the uri
        public const char Everything = '*';
        public static class RegexPatterns
        {
            //thank you https://www.regextester.com/23
            public static string ValidHostname = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";
        }
    }
}
