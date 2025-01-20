namespace VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
