using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Author : IEntity
    {
        public int Id { get; set; }        
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}