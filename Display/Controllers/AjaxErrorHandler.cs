using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Display.Controllers
{
    public class AjaxErrorHandler : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var response = filterContext.RequestContext.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusDescription = filterContext.Exception.Message;
            filterContext.ExceptionHandled = true;
        }
    }
}