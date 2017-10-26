using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace BooksAPI.Common
{
    public class PaginationResult<T> : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _routeName;
        private readonly int _pageNo;
        private readonly int _pageSize;
        private readonly int _recordCount;
        private readonly int _pageCount;

        public IQueryable<T> Data { get; }
        public IEnumerable<T> PagedData { get; private set; }

        public PaginationResult(IQueryable<T> data, HttpRequestMessage request, string routeName, int pageNo, int pageSize)
        {
            Data = data;

            _request = request;
            _routeName = routeName;
            _pageNo = pageNo < 1 ? 1 : pageNo;
            _pageSize = pageSize < 1 ? 30 : pageSize;
            _recordCount = data.Count();

            _pageCount = _recordCount > 0
                         ? (int)Math.Ceiling(_recordCount / (double)_pageSize)
                         : 0;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {           
            var skip = (_pageNo - 1) * _pageSize;

            PagedData = Data.Skip(skip)
                            .Take(_pageSize)
                            .AsEnumerable();

            var response = _request.CreateResponse(HttpStatusCode.OK, PagedData);

            response.Headers.Add("Link", BuildNavigationLinks());
            response.Headers.Add("X-Paging-Pages", _pageCount.ToString());
            response.Headers.Add("X-Paging-Records", _recordCount.ToString());

            return Task.FromResult(response);
        }

        private string BuildNavigationLinks()
        {            
            ICollection<string> links = new Collection<string>();

            links.Add(BuildPaginationLinkForUri(BuildUri(1, _pageSize), "first"));

            if (_pageNo > 1)
                links.Add(BuildPaginationLinkForUri(BuildUri(_pageNo - 1, _pageSize), "prev"));

            if (_pageNo < _pageCount)
                links.Add(BuildPaginationLinkForUri(BuildUri(_pageNo + 1, _pageSize), "next"));

            links.Add(BuildPaginationLinkForUri(BuildUri(_pageCount, _pageSize), "last"));

            return string.Join(", ", links);
        }

        private Uri BuildUri(int pageNo, int pageSize)
        {
            var httpRouteValueDictionary = new HttpRouteValueDictionary(null)
            {
                {"pageNo", pageNo},
                {"pageSize", pageSize}
            };

            return new Uri(_request.GetUrlHelper().Link(_routeName, httpRouteValueDictionary));
        }

        private string BuildPaginationLinkForUri(Uri uri, string rel)
        {
            return $"<{uri}>; rel=\"{rel}\"";
        }
    }
}