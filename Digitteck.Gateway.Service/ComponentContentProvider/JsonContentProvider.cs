using Digitteck.Gateway.Service.Abstractions;
using Digitteck.Gateway.Service.Common.Helpers;
using Digitteck.Gateway.Service.ComponentContentProvider.Models;
using Digitteck.Gateway.Service.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    internal class JsonContentProvider : IContentProvider
    {
        private readonly string _globalConfFileName;
        private string globalFileContent;
        private string[] routeFilesContent;

        internal JsonContentProvider(string fileName)
        {
            _globalConfFileName = fileName;
            routeFilesContent = new string[] { };
        }

        internal void Build()
        {
            try
            {
                this.globalFileContent = FileHelper.GetFileContent(_globalConfFileName, FilePathKind.ExecutingAssembly);

                ContentModelGlobalConfiguration globalConf = JsonConvert.DeserializeObject<ContentModelGlobalConfiguration>(this.globalFileContent);

                List<string> routeFilesContent = new List<string>();

                foreach (var routeFile in globalConf.RouteSources)
                {
                    //find filepath relative to config file
                    string directory = FileHelper.GetFileDirectory(this._globalConfFileName, FilePathKind.ExecutingAssembly);
                    var routeContent = FileHelper.GetFileContent(routeFile, directory);
                    routeFilesContent.Add(routeContent);
                }

                this.routeFilesContent = routeFilesContent.ToArray();
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.ContentProvider, ex.Message, ex);
            }
        }

        public string GetGlobalFileContent()
        {
            return globalFileContent;
        }

        public string[] GetRoutesContent()
        {
            return routeFilesContent;
        }
    }
}