namespace Factory.Models;

public class VehicleMechanic
{
  public int VehicleMechanicId { get; set; }
  public int MechanicId { get; set; }
  public int VehicleId { get; set; }
  public Vehicle Vehicle { get; set; }
  public Mechanic Mechanic { get; set; }
}
