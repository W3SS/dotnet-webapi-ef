using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace BooksAPI.Attributes
{
    public class VersionedRouteAttribute : RouteFactoryAttribute
    {
        private readonly int _allowedVersion;

        public VersionedRouteAttribute(string template, int allowedVersion) : base(template)
        {
            _allowedVersion = allowedVersion;
        }

        public override IDictionary<string, object> Constraints => new HttpRouteValueDictionary
        {
            {"version", new VersionContraint(_allowedVersion) }
        };
    }

    public class VersionContraint : IHttpRouteConstraint
    {
        private const int DefaultVersion = 1;

        private readonly int _allowedVersion;

        public VersionContraint(int allowedVersion)
        {
            _allowedVersion = allowedVersion;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection != HttpRouteDirection.UriResolution) return true;

            var version = GetVersionFromHeader(request, parameterName) ?? DefaultVersion;

            return version == _allowedVersion;
        }

        private int? GetVersionFromHeader(HttpRequestMessage request, string parameterName)
        {
            var httpHeaderValueCollection = request.Headers.Accept;

            foreach (var headerValue in httpHeaderValueCollection)
            {
                if (headerValue.MediaType == "application/json")
                {
                    var version = headerValue.Parameters.FirstOrDefault(v => v.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase));

                    int parsedVersion;
                    return int.TryParse(version.Value, out parsedVersion)
                            ? parsedVersion
                            : (int?)null;
                }
            }

            return null;
        }
    }
}