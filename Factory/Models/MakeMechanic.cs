namespace Factory.Models;

public class MakeMechanic
{
  public int MakeMechanicId { get; set; }

  public int MakeId { get; set; }
  public int MechanicId { get; set; }

  public Make Make { get; set; }

  public Mechanic Mechanic { get; set; }

}
