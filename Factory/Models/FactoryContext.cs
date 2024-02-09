using Microsoft.EntityFrameworkCore;

namespace Factory.Models;
public class FactoryContext : DbContext
{
  public DbSet<Mechanic> Mechanics { get; set; }
  public DbSet<Vehicle> Vehicles { get; set; }

  public DbSet<VehicleMechanic> VehicleMechanics { get; set; }
  public FactoryContext(DbContextOptions options) : base(options) { }

}
