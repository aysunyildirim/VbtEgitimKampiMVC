using VbtEgitimKampiMVC.Core.Domain.Enums;

namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; }
    public VehicleType VehicleType { get; set; }
    public string Color { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public int ParkingAreaId { get; set; }
    public VehicleStatus Status { get; set; }
}
