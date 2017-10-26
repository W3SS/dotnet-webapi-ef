using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using BooksAPI.Controllers;
using BooksAPI.Models;
using BooksAPI.Tests.Common;
using DataAccess;
using Domain;
using FizzWare.NBuilder;
using FluentAssertions;
using Marvin.JsonPatch;
using NSubstitute;
using NUnit.Framework;

namespace BooksAPI.Tests.Controllers
{
    [TestFixture]
    public class BooksControllerTests
    {
        private BooksController _booksController;
        private IRepository<Book> _repositoryMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = Substitute.For<IRepository<Book>>();
            _booksController = new BooksController(_repositoryMock);
        }
 
        [Test]
        public async Task Get_ReturnsAllBooks()
        {
            //Arrange
            var books = Builder<Book>.CreateListOfSize(2)
                        .All()
                            .With(x => x.Author = new Author())
                            .With(x => x.Publisher = new Publisher())
                        .Build().AsQueryable();

            _repositoryMock.GetAll().Returns(books);

            //Act
            var httpActionResult = await _booksController.Get();

            //Assert
            var results = httpActionResult.ToType<List<BookModel>>();
            results.Count.Should().Be(2);
        }

        [Test]
        public async Task GetBook_BookExists_ReturnsBook()
        {
            //Arrange
            var book = Builder<Book>.CreateNew()
                            .With(x => x.Author = new Author())
                            .With(x => x.Publisher = new Publisher())
                        .Build();

            _repositoryMock.GetById(Arg.Any<int>()).Returns(book);

            //Act
            var httpActionResult = await _booksController.Get(0);

            //Assert
            var results = httpActionResult.ToType<BookModel>();
            results.Id.Should().Be(book.Id);
        }

        [Test]
        public async Task GetBook_BookDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            _repositoryMock.GetById(Arg.Any<int>()).Returns((Book)null);

            //Act
            var httpActionResult = await _booksController.Get(0);

            //Assert
            httpActionResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetBooksByAuthor_ReturnsOk()
        {
            //Arrange
            var books = new Book[]{};

            _repositoryMock.Query(Arg.Any<Expression<Func<Book, bool>>>()).Returns(books);

            //Act
            var httpActionResult = await _booksController.GetBooksByAuthor(string.Empty);

            //Assert
            httpActionResult.Should().BeOfType<OkNegotiatedContentResult<List<BookModel>>>();
        }

        [Test]
        public async Task GetBooksByAuthor_WithAuthor_ReturnsBooksForMatchingAuthor()
        {
            //Arrange
            var books = Builder<Book>.CreateListOfSize(2)
                        .All()
                            .With(x => x.Author = new Author())
                            .With(x => x.Publisher = new Publisher())
                        .Build();

            _repositoryMock.Query(Arg.Any<Expression<Func<Book, bool>>>()).Returns(books);

            //Act
            var httpActionResult = await _booksController.GetBooksByAuthor(string.Empty);

            //Assert
            var results = httpActionResult.ToType<List<BookModel>>();
            results.Count.Should().Be(2);
        }

        [Test]
        public async Task Post_WithId_ReturnsBadRequest()
        {
            //Arrange
            var bookModel = Builder<BookModel>.CreateNew()
                                .With(x => x.Author = new AuthorModel())
                                .With(x => x.Publisher = new PublisherModel())
                            .Build();

            //Act
            var httpActionResult = await _booksController.Post(bookModel);

            //Assert
            httpActionResult.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [Test]
        public async Task Post_WithNewBook_StoresBook()
        {
            //Arrange
            var bookModel = Builder<BookModel>.CreateNew()
                                .With(x => x.Id = null)
                                .With(x => x.Author = new AuthorModel())
                                .With(x => x.Publisher = new PublisherModel())
                            .Build();

            //Act
            await _booksController.Post(bookModel);

            //Assert
            Received.InOrder(async () =>
            {
                await _repositoryMock.Received().InsertOrUpdate(Arg.Any<Book>());
            });
        }

        [Test]
        public async Task Post_WithNewBook_ReturnsCreated()
        {
            //Arrange
            var bookModel = Builder<BookModel>.CreateNew()
                                .With(x => x.Id = null)
                                .With(x => x.Author = new AuthorModel())
                                .With(x => x.Publisher = new PublisherModel())
                            .Build();

            //Act
            var httpActionResult = await _booksController.Post(bookModel);

            //Assert
            var statusCodeResult = (StatusCodeResult)httpActionResult;
            statusCodeResult.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async Task Patch_BookDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            const int bookId = 1;
            var patch = new JsonPatchDocument<BookModel>();
            _repositoryMock.GetById(Arg.Any<int>()).Returns((Book)null);

            //Act
            var httpActionResult = await _booksController.Patch(bookId, patch);

            //Assert
            httpActionResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Patch_BookExistsChangeTitle_ReturnsBookModelWithChange()
        {
            //Arrange
            const string oldTitle = "Old Title";
            const string newTitle = "New Title";

            var book = Builder<Book>.CreateNew()
                            .With(x => x.Title = oldTitle)
                            .With(x => x.Author = new Author())
                            .With(x => x.Publisher = new Publisher())                            
                        .Build();

            _repositoryMock.GetById(Arg.Any<int>()).Returns(book);

            var patch = new JsonPatchDocument<BookModel>();
            patch.Replace(x => x.Title, newTitle);

            //Act
            var httpActionResult = await _booksController.Patch(book.Id, patch);

            //Assert
            var result = httpActionResult.ToType<BookModel>();
            result.Title.Should().Be(newTitle);
        }

        [Test]
        public async Task Patch_BookExistsChangeTitle_StoresBook()
        {
            //Arrange
            const string newTitle = "New Title";

            var book = Builder<Book>.CreateNew()
                            .With(x => x.Author = new Author())
                            .With(x => x.Publisher = new Publisher())
                        .Build();

            _repositoryMock.GetById(Arg.Any<int>()).Returns(book);

            var patch = new JsonPatchDocument<BookModel>();
            patch.Replace(x => x.Title, newTitle);

            //Act
            await _booksController.Patch(book.Id, patch);

            //Assert
            Received.InOrder(async () =>
            {
                await _repositoryMock.Received().InsertOrUpdate(Arg.Is<Book>(x => x.Id == book.Id && x.Title == newTitle));
            });            
        }

        [Test]
        public async Task Delete_BookDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            const int id = 1;
            _repositoryMock.GetById(Arg.Any<int>()).Returns((Book)null);

            //Act
            var httpActionResult = await _booksController.Delete(id);

            //Assert
            httpActionResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Delete_BookDoesExists_ReturnsNoContent()
        {
            //Arrange
            var book = Builder<Book>.CreateNew().Build();

            _repositoryMock.GetById(Arg.Any<int>()).Returns(book);

            //Act
            var httpActionResult = await _booksController.Delete(book.Id);

            //Assert
            var statusCodeResult = (StatusCodeResult)httpActionResult;
            statusCodeResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}