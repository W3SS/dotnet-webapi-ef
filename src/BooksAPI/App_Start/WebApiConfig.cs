using System.Web.Http;
using System.Web.Http.Routing;
using BooksAPI.Constraint;

namespace BooksAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Add custom constraint
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("fullname", typeof(FullnameConstraint));

            // Enable attribute routing
            config.MapHttpAttributeRoutes(constraintResolver);

            // Disable default route map, to enforce attribute routing
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            
            // We don't need the xml formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
