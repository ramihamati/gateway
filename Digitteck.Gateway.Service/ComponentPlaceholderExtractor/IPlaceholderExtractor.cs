using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public interface IPlaceholderExtractor
    {
        List<Placeholder> GetPlaceholdersFromTemplate(TemplatePathAndQueryObject template);
        List<PlaceholderValue> ExtractPlaceholderValuesFromQuery(TemplatePathAndQueryObject template, PathAndQueryObject query);
        PathAndQueryObject ReplacePlaceholdersWithValues(TemplatePathAndQueryObject pathAndQuery, List<PlaceholderValue> placeholderValues);
        bool IsProperSuperset(ReadOnlySpan<Placeholder> superset, ReadOnlySpan<Placeholder> set);
        QueryObject ReplacePlaceholdersWithValues(QueryObject queryObject, List<PlaceholderValue> placeholderValues);
        PathObject ReplacePlaceholdersWithValues(PathObject pathObject, List<PlaceholderValue> placeholderValues);
        bool HasPlaceholders(string value);
    }
}
