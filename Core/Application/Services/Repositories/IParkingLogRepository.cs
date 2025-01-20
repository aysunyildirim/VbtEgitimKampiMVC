using VbtEgitimKampiMVC.Core.Domain.Entities;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Application.Services.Repositories;

public interface IParkingLogRepository : IAsyncRepository<ParkingLog, int>, IRepository<ParkingLog, int>
{
}
