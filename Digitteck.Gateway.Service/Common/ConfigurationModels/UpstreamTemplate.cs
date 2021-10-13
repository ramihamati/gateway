
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.Models
{
    public class UpstreamTemplate
    {
        #region [Original values as in Upstream]

        /// <summary>
        /// A template defining the shape of the request path+query
        /// <para>/movies?Name={movieName}</para>
        /// <para>/movies/{ListId}?{movieName}</para>
        /// </summary>
        public TemplatePathAndQueryObject EntryTemplate { get; set; }

        /// <summary>
        /// The method to match an incoming request
        /// </summary>
        public HttpMethodType HttpMethodType { get; set; }

        #endregion
        /// <summary>
        /// Regex patterns created from the <see cref="EntryTemplate"/> used to match the HttpRequest path to the template
        /// <para>E.G. From the template "api/movies/{placeholder}" will match the request "api/movies/any"</para>
        /// </summary>
       // public TemplateRegexPattern RegexTemplate { get; set; }

        /// <summary>
        /// Placeholders extracted from the template
        /// <para>E.G. From the template "api/movies/{placeholder}" will extract the placeholder with the name "placeholder"</para>
        /// </summary>
        public List<Placeholder> TemplatePlaceholders { get; set; }
    }
}
