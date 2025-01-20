using Microsoft.AspNetCore.Authentication;
using VbtEgitimKampiMVC.Core.Application.Services.Repositories;
using VbtEgitimKampiMVC.Core.Domain.Entities;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Context;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;
namespace VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories;

public class UserRepository : EfRepositoryBase<User, int, AppDbContext>, IUserRepository
{
    public UserRepository(AppDbContext context, IConfiguration configuration = null) : base(context, configuration)
    {
    }
}