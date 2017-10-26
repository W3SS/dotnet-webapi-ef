using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Routing;

namespace BooksAPI.Constraint
{
    public class FullnameConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (values.TryGetValue(parameterName, out var value) && value != null)
            {
                return Regex.Match(Convert.ToString(value), "^[A-z\\s]+$").Success;
            }

            return false;
        }
    }
}