using System.Data.Entity.Migrations;
using Domain;

namespace DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BooksContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BooksContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var author1 = new Author { Id = 1, Name = "Amy Brown" };
            var author2 = new Author { Id = 2, Name = "Cathy Dunne" };
            var author3 = new Author { Id = 3, Name = "Elle Fergusson" };

            var publisher1 = new Publisher { Id = 1, Name = "O' Reilly"};
            var publisher2 = new Publisher { Id = 2, Name = "Microsoft Press"};
            var publisher3 = new Publisher { Id = 3, Name = "Wescox"};
            var publisher4 = new Publisher { Id = 4, Name = "Apress"};

            context.Books.AddOrUpdate(x => x.Id,
                new Book { Id = 1, Isbn = "11111", Title = "New Zealand", Author = author1, Publisher = publisher1 },
                new Book { Id = 2, Isbn = "22222", Title = "Red", Author = author2, Publisher = publisher2 },
                new Book { Id = 3, Isbn = "33333", Title = "Square", Author = author3, Publisher = publisher3 },
                new Book { Id = 4, Isbn = "44444", Title = "Blue", Author = author2, Publisher = publisher2 },
                new Book { Id = 5, Isbn = "55555", Title = "Circle", Author = author3, Publisher = publisher4 }
                );
        }
    }
}
