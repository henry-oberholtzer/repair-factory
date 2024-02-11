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

  public async Task<IActionResult> Index()
  {
    List<Make> makes = await _db.Makes
      .Include(m => m.MakeVehicles)
      .Include(m => m.MakeMechanics)
      .ToListAsync();
    return View(makes);
  }

  [HttpPost]
  public ActionResult Create()
  {
    
    return View();
  }

}
