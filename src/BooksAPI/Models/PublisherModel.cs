using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class PublisherModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}