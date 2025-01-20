using VbtEgitimKampiMVC.Core.Application.Services.Repositories;
using VbtEgitimKampiMVC.Core.Domain.Entities;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Context;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories;

public class RoleRepository : EfRepositoryBase<Role, int, AppDbContext>, IRoleRepository
{
    public RoleRepository(AppDbContext context, IConfiguration configuration = null) : base(context, configuration)
    {
    }
}