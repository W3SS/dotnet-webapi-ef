using BooksAPI.Models;
using Domain;

namespace BooksAPI.Common
{
    public static class EntityExtensions
    {
        public static BookModel ToModel(this Book book)
        {
            return new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                Author = book.Author.ToModel(),
                Publisher = book.Publisher.ToModel()
            };
        }

        public static AuthorModel ToModel(this Author author)
        {
            return new AuthorModel
            {
                Id = author.Id,
                Name = author.Name
            };
        }

        public static PublisherModel ToModel(this Publisher publisher)
        {
            return new PublisherModel
            {
                Id = publisher.Id,
                Name = publisher.Name
            };
        }
    }
}