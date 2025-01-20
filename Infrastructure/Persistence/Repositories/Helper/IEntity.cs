namespace VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
