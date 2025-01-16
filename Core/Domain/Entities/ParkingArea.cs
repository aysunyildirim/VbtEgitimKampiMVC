using VbtEgitimKampiMVC.Core.Domain.Enums;

namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class ParkingArea
{

    public int Id { get; set; }
    public string Name { get; set; }
    public ParkingLocation Location { get; set; }
    public int TotalSpaces { get; set; }
    public int AvailableSpaces { get; set; }
    public bool IsActive { get; set; }

    public ParkingFee ParkingFee { get; set; }
    public int ParkingFeeId { get; set; }

}
