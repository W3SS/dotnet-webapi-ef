using System.Web.Http;
using System.Web.Http.Results;

namespace BooksAPI.Tests.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the type from the content property of a OkNegotiatedContentResult.
        /// </summary>
        /// <typeparam name="T">The type expected.</typeparam>
        /// <param name="httpActionResult">The IHttpActionResult which contains the type.</param>
        /// <returns>Type specified</returns>
        public static T ToType<T>(this IHttpActionResult httpActionResult)
        {
            return httpActionResult is OkNegotiatedContentResult<T> contentResult ? contentResult.Content : default;
        }
    }
}
