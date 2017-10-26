namespace Domain
{
    public enum EntityState
    {
        Added,
        Deleted,
        Detatched,
        Modified,
        Unchanged
    }

    public interface IEntity
    {
        EntityState EntityState { get; set; } 
    }
}