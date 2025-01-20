using VbtEgitimKampiMVC.Core.Domain.Entities;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Application.Services.Repositories;

public interface IParkingAreaRepository : IAsyncRepository<ParkingArea, int>, IRepository<ParkingArea, int>
{



}
