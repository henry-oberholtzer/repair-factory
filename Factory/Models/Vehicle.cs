using System.ComponentModel.DataAnnotations;

namespace Factory.Models;
public class Vehicle
{
    [Range(0, int.MaxValue)]
    public int VehicleId { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Required]
    public string Make { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Required]
    public string Model { get; set; }

    [Range(1800, 3000)]
    [Required]
    [Display(Name = "Model Year")]
    public int ModelYear { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Required]
    public string Condition { get; set; }

    public DateTime DateAdded { get; set; }

    public List<VehicleMechanic> VehicleMechanics { get; set; }

}