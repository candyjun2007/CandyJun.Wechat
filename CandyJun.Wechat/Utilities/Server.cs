using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CandyJun.Wechat.Utilities
{
    public static class Server
    {
        private static string _appDomainAppPath;
        public static string AppDomainAppPath
        {
            get
            {
                if (_appDomainAppPath == null)
                {
#if NET45
                    _appDomainAppPath = HttpRuntime.AppDomainAppPath;
#else
                    _appDomainAppPath = AppContext.BaseDirectory; //dll所在目录：;
#endif
                }
                return _appDomainAppPath;
            }
            set
            {
                _appDomainAppPath = value;
#if NETSTANDARD1_6 || NETSTANDARD2_0
                if (!_appDomainAppPath.EndsWith("\\"))
                {
                    _appDomainAppPath += "\\";
                }
#endif
            }
        }

        public static string GetMapPath(string virtualPath)
        {
            if (virtualPath == null)
            {
                return "";
            }
            else if (virtualPath.StartsWith("~/"))
            {
                return virtualPath.Replace("~/", AppDomainAppPath).Replace("/", "\\");
            }
            else
            {
                return Path.Combine(AppDomainAppPath, virtualPath.Replace("/", "\\"));
            }
        }

        public static HttpContext HttpContext
        {
            get
            {
                HttpContext context = new DefaultHttpContext();
                return context;
            }
        }
    }
}
