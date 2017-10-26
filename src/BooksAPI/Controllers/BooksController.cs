using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BooksAPI.Attributes;
using BooksAPI.Common;
using BooksAPI.Models;
using DataAccess;
using Domain;
using Marvin.JsonPatch;
using Marvin.JsonPatch.Exceptions;

namespace BooksAPI.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Books api for getting/finding/adding/deleting library books.
    /// </summary>
    [RoutePrefix("api/library/books")]
    public class BooksController : ApiController
    {
        private readonly IRepository<Book> _repository;

        /// <inheritdoc />
        /// <summary>
        /// Default constructor. Requires books repository.
        /// </summary>
        /// <param name="repository"></param>
        public BooksController(IRepository<Book> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all books.
        /// </summary>
        /// <returns>Ok with all books.</returns>
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var books = await _repository.GetAll();

            return Ok(books.Select(x => x.ToModel()).ToList());
        }

        /// <summary>
        /// Get book for given book id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok with matching book OR NotFound.</returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var book = await _repository.GetById(id);

            if (book == null)
                return NotFound();

            return Ok(book.ToModel());
        }

        /// <summary>
        /// Get books for given Author's name.
        /// </summary>
        /// <param name="author"></param>
        /// <returns>Ok with list of books.</returns>
        [Route("~/api/library/authors/{author:fullname}/books")]
        public async Task<IHttpActionResult> GetBooksByAuthor(string author)
        {
            var books = await _repository.Query(x => x.Author.Name == author);

            return Ok(books.Select(x => x.ToModel()).ToList());
        }

        /// <summary>
        /// Add a book.
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Created status for successful post.</returns>
        [Route(""), ValidateModelState]
        public async Task<IHttpActionResult> Post(BookModel book)
        {
            if (book.Id != null)
                return BadRequest("Cannot add book as it already has an id.");

            await _repository.InsertOrUpdate(book.ToEntity());

            return StatusCode(HttpStatusCode.Created);
        }

        /// <summary>
        /// Update existing book.
        /// </summary>
        /// <param name="id">Id of book.</param>
        /// <param name="patch">JSON patch operations in body.</param>
        /// <returns>Ok with updated book.</returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Patch(int id, JsonPatchDocument<BookModel> patch)
        {
            var book = await _repository.GetById(id);
            if (book == null)
                return NotFound();

            try
            {
                var bookModel = book.ToModel();
                patch.ApplyTo(bookModel);
                await _repository.InsertOrUpdate(bookModel.ToEntity());

                return Ok(bookModel);
            }
            catch (JsonPatchException exception)
            {
                return BadRequest($"An error occured whilst updating book. Status {exception.SuggestedStatusCode}. {exception.Message}.");
            }
        }

        /// <summary>
        /// Deletes book for given id.
        /// </summary>
        /// <param name="id">Book id.</param>
        /// <returns>No content status for successful deletion.</returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var book = await _repository.GetById(id);
            if (book == null)
                return NotFound();

            await _repository.Delete(id);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}