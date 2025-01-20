﻿using VbtEgitimKampiMVC.Core.Domain.Entities;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Application.Services.Repositories;

public interface IParkingFeeRepository : IAsyncRepository<ParkingFee, int>, IRepository<ParkingFee, int>
{
}
