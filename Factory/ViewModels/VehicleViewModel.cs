using System.ComponentModel.DataAnnotations;

namespace Factory.ViewModels;

public class VehicleViewModel
{
    [Range(0, int.MaxValue)]
    public int VehicleId { get; set; }

    [RegularExpression(@"^[A-Z\d]{1,7}", ErrorMessage = "Please double check the license plate")]
    [Display(Name = "License Plate")]
    [Required(ErrorMessage = "The License Plate is Required")]
    public string LicensePlate { get; set; }

    [Range(0, int.MaxValue)]
    [Required]
    [Display(Name = "Make")]
    public int MakeId { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Display(Name = "If the make you're looking for is not listed, add a new one here")]
#nullable enable
    public string? NewMake { get; set; }

#nullable disable

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Required]
    public string Model { get; set; }

    [Range(1800, 3000)]
    [Required]
    [Display(Name = "Model Year")]
    public int ModelYear { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    public string Color { get; set; }

    [StringLength(255, ErrorMessage = "The {0} cannot be longer than {1} characters")]
    [Required]
    public string Condition { get; set; }
}
