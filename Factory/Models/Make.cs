using System.ComponentModel.DataAnnotations;

namespace Factory.Models;

public class Make
{
  [Range(0, int.MaxValue)]
  public int MakeId { get; set; }

  [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
  [Required]
  public string Name { get; set; }

  public List<MakeMechanic> MakeMechanics { get; set; }

  public List<MakeVehicle> MakeVehicles { get; set; }

}
