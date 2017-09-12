using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Display
{
    public static class HelperExtensions
    {
        public static string GetStaticUrl(this UrlHelper helper, string path)
        {
            return ConfigurationManager.AppSettings["StaticHost"] + path;
        }
    }
}