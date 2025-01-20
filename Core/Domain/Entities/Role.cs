using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class Role : Entity<int>
{
    public string Name { get; set; }  // Role name (e.g., "Admin", "Visitor", "Staff")
    public string Description { get; set; }  // Description of the role
    public ICollection<User> Users { get; set; }  // List of users with this role

}
