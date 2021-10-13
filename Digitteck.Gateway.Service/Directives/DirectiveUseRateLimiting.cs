using Digitteck.Gateway.Service.Abstractions;
using Digitteck.Gateway.Service.Attributes;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    [DirectiveName("UseRateLimiting")]
    public class DirectiveUseRateLimiting : DirectiveCore
    {
        public List<string> ClientWhitelist { get; set; }

        public string Period { get; set; }

        public int PeriodTimespan { get; set; }

        public int Limit { get; set; }

        public DirectiveUseRateLimiting() 
        {
            this.ClientWhitelist = new List<string>();
        }
    }

}
