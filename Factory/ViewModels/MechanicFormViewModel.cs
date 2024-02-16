using System.ComponentModel.DataAnnotations;
using Factory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Factory.ViewModels;

public class MechanicFormViewModel
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

  
  public SelectList MakeSelectList { get; set; }

  [Display(Name = "Makes Licensed to Service")]
  public List<int> SelectedMakes { get; set; }
  public bool FormEdit { get; set; } = false;

  public MechanicFormViewModel()
  {

  }
  public MechanicFormViewModel(SelectList makeSelectList)
  {
    MakeSelectList = makeSelectList;
  }
  public MechanicFormViewModel(Mechanic mechanic, SelectList makeSelectList)
  {
    MechanicId = mechanic.MechanicId;
    FirstName = mechanic.FirstName;
    LastName = mechanic.LastName;
    About = mechanic.About;
    MakeSelectList = makeSelectList;
  }

  public MechanicFormViewModel(Mechanic mechanic, SelectList makeSelectList, bool formEdit)
  {
    MechanicId = mechanic.MechanicId;
    FirstName = mechanic.FirstName;
    LastName = mechanic.LastName;
    About = mechanic.About;
    MakeSelectList = makeSelectList;
    FormEdit = formEdit;
  }

  public Mechanic ToMechanic()
  {
    return new Mechanic{
      FirstName = FirstName,
      LastName = LastName,
      About = About,
      DateAdded = DateTime.Now,
    };
  }
  public Mechanic ToMechanic(Mechanic mechanic)
  {
    mechanic.FirstName = FirstName;
    mechanic.LastName = LastName;
    mechanic.About = About;
    return mechanic;
  }
}
