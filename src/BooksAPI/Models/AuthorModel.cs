using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class AuthorModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}