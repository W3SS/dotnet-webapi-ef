using BooksAPI.Models;
using Domain;

namespace BooksAPI.Common
{
    public static class ModelExtensions
    {
        public static Book ToEntity(this BookModel bookModel)
        {
            var entity = new Book
            {
                Id = bookModel.Id ?? 0,
                Isbn = bookModel.Isbn,
                Title = bookModel.Title,
                Author = bookModel.Author.ToEntity(),
                Publisher = bookModel.Publisher.ToEntity(),
                EntityState = bookModel.Id == null ? EntityState.Added : EntityState.Modified
            };

            return entity;
        }

        public static Author ToEntity(this AuthorModel authorModel)
        {
            var entity = new Author
            {
                Id = authorModel.Id ?? 0,
                Name = authorModel.Name,
                EntityState = authorModel.Id == null ? EntityState.Added : EntityState.Modified
            };

            return entity;
        }

        public static Publisher ToEntity(this PublisherModel publisherModel)
        {
            var publisher = new Publisher
            {
                Id = publisherModel.Id ?? 0,
                Name = publisherModel.Name,
                EntityState = publisherModel.Id == null ? EntityState.Added : EntityState.Modified
            };

            return publisher;
        }
    }
}