namespace VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper
{
    public interface IEntityTimestamps
    {
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        DateTime? DeletedDate { get; set; }

        string? CreUser { get; set; }
        string? ModUser { get; set; }

    }

}
