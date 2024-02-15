using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
      if (!ModelState.IsValid)
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
      List<Vehicle> unselected = await _db.Vehicles
      .Include(v => v.VehicleMechanics)
      .Where(v => !v.VehicleMechanics.Any(vm => vm.MechanicId == id))
      .ToListAsync();
      SelectList vehiclesSelectList = new(unselected, "VehicleId", "YearMakeModelPlate");

      Mechanic mechanic = await _db
      .Mechanics
      .Include(m => m.VehicleMechanics)
      .ThenInclude(join => join.Vehicle)
      .FirstOrDefaultAsync(m => m.MechanicId == id);
      Dictionary<string, object> model = new() {
            {"selectList", vehiclesSelectList},
            {"joinEntity", new VehicleMechanic()},
            {"mechanic", mechanic}
        };
      return View(model);
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
      return RedirectToAction("Details", new { id = mechanic.MechanicId});
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

    [HttpPost]
    public async Task<IActionResult> AssignVehicle(VehicleMechanic vm)
    {
#nullable enable
        VehicleMechanic? joinEntity = await _db.VehicleMechanics
        .FirstOrDefaultAsync(join => join.VehicleId == vm.VehicleId && join.MechanicId == vm.MechanicId);
#nullable disable
        if (joinEntity == null && vm.VehicleId != 0)
        {
            _db.VehicleMechanics.Add(vm);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = vm.MechanicId });
        }
        return RedirectToAction("Details", new { id = vm.MechanicId });
    }

    [HttpPost]
    public async Task<IActionResult> UnassignVehicle(int id)
    {
        VehicleMechanic joinEntity = await _db.VehicleMechanics.FirstOrDefaultAsync(vm => vm.VehicleMechanicId == id);
        int mechanicId = joinEntity.MechanicId;
        _db.VehicleMechanics.Remove(joinEntity);
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", new { id = mechanicId });
    }
  }
