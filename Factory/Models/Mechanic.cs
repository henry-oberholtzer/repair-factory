using System.ComponentModel.DataAnnotations;

namespace Factory.Models;
public class Mechanic
{
    public int MechanicId { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Display(Name = "First Name")]
    [Required]
    public string FirstName { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Display(Name = "Last Name")]
    [Required]
    public string LastName { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Required]
    public string About { get; set; }
    public DateTime DateAdded { get; set; }

    public List<VehicleMechanic> VehicleMechanics { get; set; }
}
