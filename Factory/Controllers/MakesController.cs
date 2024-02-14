using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using Microsoft.EntityFrameworkCore;

namespace Factory.Controllers;
public class MakesController : Controller
{
  private readonly FactoryContext _db;

  public MakesController(FactoryContext db)
  {
    _db = db;
  }
  public Dictionary <string, object> MakeFormModel(Make make, string action)
  {
    return new Dictionary<string, object> {
      {"Make", make},
      {"Action", action}
    };
  }

  public bool CheckForDuplicates(Make make)
  {
#nullable enable
  Make? existing = _db.Makes.FirstOrDefault(m => m.Name.ToLower() == make.Name.ToLower());
#nullable disable
  if (existing == null)
  {
    return false;
  }
  else
  {
    return true;
  }
  }
  public async Task<IActionResult> Index()
  {
    List<Make> makes = await _db.Makes
      .Include(m => m.MakeVehicles)
      .Include(m => m.MakeMechanics)
      .ToListAsync();
    return View(makes);
  }

  public ActionResult Create()
  {
    
    return View(MakeFormModel(new Make(), "Create"));
  }

  [HttpPost]
  public ActionResult Create(Make make)
  {
    if(!ModelState.IsValid)
    {
      return View(MakeFormModel(make, "Create"));
    }
    if (CheckForDuplicates(make) == false)
    {
      _db.Makes.Add(make);
      _db.SaveChanges();
    }
    return RedirectToAction("Index");
  }

  public ActionResult Edit(int id)
  {
    Make target = _db.Makes.FirstOrDefault(m => m.MakeId == id);
    return View(MakeFormModel(target, "Edit"));
  }

  [HttpPost]
  public ActionResult Edit(Make make)
  {
    if(!ModelState.IsValid)
    {
      return View(MakeFormModel(make, "Edit"));
    }
    if (CheckForDuplicates(make) == false)
    {
      _db.Makes.Update(make);
      _db.SaveChanges();
    }
    return RedirectToAction("Index");
  }

  [HttpPost]
  public ActionResult Delete(int id)
  {
    Make target = _db.Makes
    .Include(m => m.MakeMechanics)
    .Include(m => m.MakeVehicles)
    .FirstOrDefault(m => m.MakeId == id);
    if(target.MakeVehicles.Count == 0 && target.MakeMechanics.Count == 0)
    {
      _db.Makes.Remove(target);
      _db.SaveChanges();
    }
    return RedirectToAction("Index");
  }

}
