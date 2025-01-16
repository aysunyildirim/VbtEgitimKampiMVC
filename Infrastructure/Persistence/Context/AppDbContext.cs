using Microsoft.EntityFrameworkCore;
using VbtEgitimKampiMVC.Core.Domain.Entities;

namespace VbtEgitimKampiMVC.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ParkingArea> ParkingAreas { get; set; }
    public DbSet<ParkingFee> ParkingFees { get; set; }
    public DbSet<ParkingLog> ParkingLogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
