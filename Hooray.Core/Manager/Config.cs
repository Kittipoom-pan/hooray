using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Manager
{
    public class Config : IConfig
    {
        private readonly Resource _appSettings;
        public Config( IOptions<Resource> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        //public string Encrypt(string plainText)
        //{
        //    (...)
        //}
    }
}
