using System.ComponentModel.DataAnnotations;

namespace Factory.Models;
public class Stylist
{
    public int StylistId { get; set; }

    [MinLength(1)]
    [MaxLength(255)]
    [Display(Name = "Stylist Name")]
    [Required]
    public string Name { get; set; }

    [MinLength(1)]
    [MaxLength(255)]
    [Display(Name = "About")]
    [Required]
    public string About { get; set; }
    [Required]
    public DateTime DateAdded { get; set; }
    public List<Client> Clients { get; set; }
}
