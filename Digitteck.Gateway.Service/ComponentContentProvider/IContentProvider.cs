using Digitteck.Gateway.Service.Abstractions;
using Digitteck.Gateway.Service.Common.Helpers;
using Digitteck.Gateway.Service.ComponentContentProvider.Models;
using Digitteck.Gateway.Service.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public interface IContentProvider
    {
        string GetGlobalFileContent();
        string[] GetRoutesContent();
    }
}