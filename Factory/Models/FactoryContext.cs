using Microsoft.EntityFrameworkCore;

namespace Factory.Models;
public class FactoryContext : DbContext
{
  public DbSet<Mechanic> Mechanics { get; set; }
  public DbSet<Vehicle> Vehicles { get; set; }

  public DbSet<VehicleMechanic> VehicleMechanics { get; set; }

  public DbSet<Make> Makes { get; set; }

  public DbSet<MakeMechanic> MakeMechanics { get; set; }

  public DbSet<MakeVehicle> MakeVehicles { get; set; }

  public FactoryContext(DbContextOptions options) : base(options) { }

}
