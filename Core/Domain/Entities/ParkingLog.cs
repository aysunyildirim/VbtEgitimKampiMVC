using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class ParkingLog : Entity<int>

{
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public int ParkingAreaId { get; set; }
    public ParkingArea ParkingArea { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal Fee { get; set; }

}
