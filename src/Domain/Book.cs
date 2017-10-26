using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Book : IEntity
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual Author Author { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}
