using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using Microsoft.EntityFrameworkCore;

namespace Factory.Controllers;

  public class MechanicsController : Controller
  {
    private readonly FactoryContext _db;

    public MechanicsController(FactoryContext db)
    {
      _db = db;
    }
    public static Dictionary<string, object> MechanicFormModel(Mechanic mechanic, string action) {
      return new Dictionary<string, object> {
        {"Mechanic", mechanic},
        {"Action", action}
      };
    }
    public async Task<IActionResult> Index()
    {
      List<Mechanic> mechanics = await _db.Mechanics.Include(m => m.VehicleMechanics).ToListAsync();
      return View(mechanics);
    }

    public ActionResult Create()
    {
      return View(MechanicFormModel(new Mechanic(), "Create"));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Mechanic mechanic)
    {
      if (ModelState.IsValid)
      {
        mechanic.DateAdded = DateTime.Now;
        _db.Mechanics.Add(mechanic);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(MechanicFormModel(mechanic, "Create"));
    }

    public async Task<IActionResult> Details(int id)
    {
      Mechanic mechanic = await _db
      .Mechanics
      .Include(m => m.VehicleMechanics)
      .ThenInclude(join => join.Vehicle)
      .FirstOrDefaultAsync(m => m.MechanicId == id);
      return View(mechanic);
    }

    public async Task<IActionResult> Edit(int id)
    {
      Mechanic thisMechanic = await _db
      .Mechanics
      .FirstOrDefaultAsync(m => m.MechanicId == id);
      return View(MechanicFormModel(thisMechanic, "Edit"));
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Mechanic mechanic)
    {
      if (ModelState.IsValid) 
      {
      _db.Mechanics.Update(mechanic);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
      }
      return View(MechanicFormModel(mechanic, "Edit"));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      Mechanic thisMechanic = await _db
      .Mechanics
      .FirstOrDefaultAsync(m => m.MechanicId == id);
      _db.Mechanics.Remove(thisMechanic);
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }
  }
