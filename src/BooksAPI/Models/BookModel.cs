using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class BookModel
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Isbn { get; set; }

        [Required]
        public AuthorModel Author { get; set; }
        [Required]
        public PublisherModel Publisher { get; set; }
    }
}