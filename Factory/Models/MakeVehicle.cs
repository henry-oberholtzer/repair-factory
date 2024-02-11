namespace Factory.Models;

public class MakeVehicle
{
  public int MakeVehicleId { get; set; }
  
  public int MakeId { get; set; }

  public int VehicleId { get; set; }

  public Make Make { get; set; }

  public Vehicle Vehicle { get; set; }
}
