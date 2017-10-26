using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BooksAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ActionArguments.ContainsValue(null)) return;

            var message = $"The argument cannot be null: {string.Join(",", actionContext.ActionArguments.Where(i => i.Value == null).Select(i => i.Key))}";
            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }
    }
}