namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class ParkingLog

{

    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public int ParkingAreaId { get; set; }
    public ParkingArea ParkingArea { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal Fee { get; set; }

}
