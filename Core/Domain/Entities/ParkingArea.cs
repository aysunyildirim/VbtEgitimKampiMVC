using VbtEgitimKampiMVC.Core.Domain.Enums;
using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class ParkingArea : Entity<int>
{

    public string Name { get; set; }
    public ParkingLocation Location { get; set; }
    public int TotalSpaces { get; set; }
    public int AvailableSpaces { get; set; }
    public bool IsActive { get; set; }

    public ParkingFee ParkingFee { get; set; }
    public int ParkingFeeId { get; set; }

}
