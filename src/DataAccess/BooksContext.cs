using System.Data.Entity;
using Domain;

namespace DataAccess
{
    internal class BooksContext : DbContext
    {
        private const string DatabaseName = "BooksDatabase";

        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BooksContext() : base(DatabaseName)
        {
            var connectionString = Common.Configuration.GetConnectionStringForKey(DatabaseName);
            Database.Connection.ConnectionString = connectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasRequired(x => x.Publisher);
            modelBuilder.Entity<Book>().HasRequired(x => x.Author);
        }
    }
}