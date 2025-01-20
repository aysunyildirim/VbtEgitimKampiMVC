using VbtEgitimKampiMVC.Core.Domain.Entities;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Application.Services.Repositories;

public interface IRoleRepository : IAsyncRepository<Role, int>, IRepository<Role, int>
{
}
