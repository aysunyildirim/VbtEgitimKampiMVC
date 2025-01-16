namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class ParkingFee
{

    public int Id { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal MonthlyRate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
