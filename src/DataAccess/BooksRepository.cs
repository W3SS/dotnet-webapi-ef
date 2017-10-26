using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;

namespace DataAccess
{
    public class BooksRepository : IRepository<Book>
    {
        public async Task<IEnumerable<Book>> GetAll()
        {
            using (var context = new BooksContext())
            {
                return await context.Books
                                    .AsNoTracking()
                                    .Include(x => x.Publisher)
                                    .Include(x => x.Author)
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Book>> Query(Expression<Func<Book, bool>> predicate)
        {
            using (var context = new BooksContext())
            {
                return await context.Books
                                    .AsNoTracking()
                                    .Include(x => x.Publisher)
                                    .Include(x => x.Author)
                                    .Where(predicate)
                                    .ToListAsync();
            }
        }

        public async Task<Book> GetById(int id)
        {
            using (var context = new BooksContext())
            {
                return await context.Books
                                    .AsNoTracking()
                                    .Include(x => x.Publisher)
                                    .Include(x => x.Author)
                                    .FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task InsertOrUpdate(Book book)
        {
            using (var context = new BooksContext())
            {
                context.Entry(book).State = book.EntityState.ToEntityFrameworkState();
                context.Entry(book.Author).State = book.Author.EntityState.ToEntityFrameworkState();
                context.Entry(book.Publisher).State = book.Publisher.EntityState.ToEntityFrameworkState();
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            using (var context = new BooksContext())
            {
                var book = context.Books.Find(id);
                if (book == null) return;

                context.Books.Remove(book);
                await context.SaveChangesAsync();
            }
        }
    }
}