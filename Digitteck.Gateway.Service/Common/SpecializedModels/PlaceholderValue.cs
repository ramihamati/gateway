using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Digitteck.Gateway.Service
{
    public class PlaceholderValue
    {
        public PlaceholderValue(Placeholder placeholder, string value)
        {
            Value = value;
            Placeholder = placeholder;
        }

        public string Value { get; }
        public Placeholder Placeholder { get; }
    }
}
