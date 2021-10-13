using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Digitteck.Gateway.Service
{
    public class PlaceholderExtractor : IPlaceholderExtractor
    {
        //char pStart = GlobalEnv.PlaceholderStart;
        //char pEnd = GlobalEnv.PlaceholderEnd;

        public bool HasPlaceholders(string value)
        {
            var s1parts = value.Split(GlobalEnv.PathDelimiter);

            for (int i = 0; i < s1parts.Length; i++)
            {
                if (IsPlaceholder(s1parts[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public QueryObject ReplacePlaceholdersWithValues(QueryObject queryObject, List<PlaceholderValue> placeholderValues)
        {
            string hydrated = queryObject;

            hydrated = ReplacePlaceholdersInString(placeholderValues, hydrated);

            return new QueryObject(hydrated);
        }

        public PathObject ReplacePlaceholdersWithValues(PathObject pathObject, List<PlaceholderValue> placeholderValues)
        {
            string hydrated = pathObject;

            hydrated = ReplacePlaceholdersInString(placeholderValues, hydrated);

            return new PathObject(hydrated);
        }

        public PathAndQueryObject ReplacePlaceholdersWithValues(TemplatePathAndQueryObject pathAndQuery, List<PlaceholderValue> placeholderValues)
        {
            string hydrated = pathAndQuery;

            hydrated = ReplacePlaceholdersInString(placeholderValues, hydrated);

            return new PathAndQueryObject(hydrated);
        }

        private bool IndexFound(int index)
        {
            return index > -1;
        }

        /// <summary>
        /// Template : "/movies/{movieName}" => [ "movieName" ]
        /// Template : "/movies/{movieName}?rating={movieRating}" => [ "movieName", "movieRating" ]
        /// </summary>
        /// <returns></returns>
        public List<Placeholder> GetPlaceholdersFromTemplate(TemplatePathAndQueryObject template)
        {
            List<Placeholder> placeHolders = new List<Placeholder>();

            //translates to @"(\{[^{}]{1,}\})"
            //means any group starting with {, ending with } and not containing { or }
            string placeHolderPattern = @"(\{[^{}&\/\?\\]{1,}\})";

            foreach (object matchItem in Regex.Matches(template, placeHolderPattern))
            {
                if (matchItem is Match match)
                {
                    if (placeHolders.Any(x => x.LongName == match.Value))
                    {
                        throw new GatewayException(ErrorCode.PlaceholderExtractor, $"A duplicate placeholder name was found in \'{template}\'. Placeholder name = \'{match.Value}\'");
                    }

                    placeHolders.Add(new Placeholder(match.Value));
                }
            }

            return placeHolders;
        }

        /// <summary>
        /// From template "/movies/{movieName}" we get "\/movies\/(?<movieName>[a-zA-Z0-9]){0,}"
        /// We use this to match incoming request and then extract values
        /// </summary>
        internal string GetRegexWithNamedGroups(TemplatePathAndQueryObject template, List<Placeholder> placeholders)
        {
            //escape minimal characters
            //string pattern = Regex.Escape(template);
            //fix - Regex.Escape does not escape forward slash and it escapes { which we don't need since it's
            //part of the placeholder that will be replaced entirely
            string pattern = template;

#pragma warning disable RCS1192 // Use regular string literal instead of verbatim string literal.
            pattern = pattern.Replace(@"\", @"\\"); //this has to be first
            pattern = pattern.Replace(@"/", @"\/");
            pattern = pattern.Replace(@"*", @"\*");
            pattern = pattern.Replace(@"+", @"\+");
            pattern = pattern.Replace(@"?", @"\?");
            pattern = pattern.Replace(@"|", @"\|");
            pattern = pattern.Replace(@"[", @"\[");
            pattern = pattern.Replace(@"]", @"\]");
            pattern = pattern.Replace(@"(", @"\(");
            pattern = pattern.Replace(@")", @"\)");
            pattern = pattern.Replace(@"^", @"\^");
            pattern = pattern.Replace(@"$", @"\$");
            pattern = pattern.Replace(@".", @"\.");
            pattern = pattern.Replace(@"#", @"\#");
#pragma warning restore RCS1192 // Use regular string literal instead of verbatim string literal.

            foreach (Placeholder placeholder in placeholders)
            {
                //replace {movieName} in template with (?<movieName>[^{}]{0,}) where [^{}] means any character not in the set
                pattern = pattern.Replace(placeholder.LongName, $"(?<{placeholder.ShortName}>"+ @"[^{}&\/\?\\]{0,})");
            }

            return pattern;
        }

        /// <summary>
        /// query : "api/movies/AmericanGangster"
        /// template : "api/movies/{movieName}
        /// result : [ { "movieName" : "AmericanGanster" } ]
        /// </summary>
        public List<PlaceholderValue> ExtractPlaceholderValuesFromQuery(TemplatePathAndQueryObject template, PathAndQueryObject query)
        {
            List<Placeholder> placeholders = GetPlaceholdersFromTemplate(template);

            string pattern = GetRegexWithNamedGroups(template, placeholders);

            MatchCollection matches = Regex.Matches(query, pattern, RegexOptions.IgnoreCase);

            List<PlaceholderValue> values = new List<PlaceholderValue>();

            foreach (Placeholder placeholder in placeholders)
            {
                string value = GetValueForGroupName(placeholder.ShortName, matches);

                values.Add(new PlaceholderValue(placeholder, value));
            }

            return values;
        }

        private string GetValueForGroupName(string groupName, MatchCollection matches)
        {
            foreach (var matchObj in matches)
            {
                if (matchObj is Match match)
                {
                    foreach (object groupObj in match.Groups)
                    {
                        if (groupObj is Group group)
                        {
                            if (group.Name == groupName)
                            {
                                return group.Value;
                            }
                        }
                    }
                }
            }

            return "";
        }

        public bool IsProperSuperset(ReadOnlySpan<Placeholder> superset, ReadOnlySpan<Placeholder> set)
        {
            for (int i = 0; i < set.Length; i++)
            {
                if (superset.BinarySearch(set[i]) < 0)
                {
                    return false;
                }
            }

            return true;
        }


        private string ReplacePlaceholdersInString(List<PlaceholderValue> placeholderValues, string hydrated)
        {
            for (int i = 0; i < placeholderValues.Count; i++)
            {
                PlaceholderValue placeholderValue = placeholderValues[i];
                //the LongName contains also the brackets e.g. {movieName}
                int index = hydrated.IndexOf(placeholderValue.Placeholder.LongName);

                //if found
                if (IndexFound(index))
                {
                    int length = placeholderValue.Placeholder.LongName.Length;

                    hydrated = $"{hydrated.Substring(0, index)}{placeholderValue.Value}{hydrated.Substring(index + length, hydrated.Length - index - length)}";
                }
            }

            return hydrated;
        }

        private bool IsPlaceholder(string s)
        {
            return s.StartsWith('{') && s.EndsWith('}');
        }
    }
}
