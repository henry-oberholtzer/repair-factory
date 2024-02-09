using System.ComponentModel.DataAnnotations;

namespace Factory.Models;
public class Client
{
    public int ClientId { get; set; }

    [MinLength(1)]
    [MaxLength(255)]
    [Display(Name = "Client Name")]
    [Required(ErrorMessage = "A name is required")]
    public string Name { get; set; }

    [MinLength(1)]
    [MaxLength(255)]
    [Display(Name = "Notes")]
    [Required(ErrorMessage = "Notes are required")]
    public string Notes { get; set; }

    public DateTime DateAdded { get; set; }

    [Display(Name = "Stylist")]
    [Required(ErrorMessage = "The client must be assigned to a stylist")]
    public int StylistId { get; set; }
    public Stylist Stylist { get; set; }

}
