using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Factory.Models;
using Factory.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Factory.Controllers;

public class MechanicsController : Controller
{
  private readonly FactoryContext _db;

  public MechanicsController(FactoryContext db)
  {
    _db = db;
  }
  public SelectList MakeSelectList()
  {
    List<Make> makeList = _db.Makes.OrderBy(m => m.Name).ToList();
    return new SelectList(makeList, "MakeId", "Name");
  }

  public SelectList MakeSelectList(List<int> selected)
  {
    List<Make> makeList = _db.Makes.OrderBy(m => m.Name).ToList();
    SelectList selectList = new(makeList, "MakeId", "Name");
    foreach (SelectListItem i in selectList)
    {
      if (selected.Contains(int.Parse(i.Value)))
      {
        i.Selected = true;
      }
    }
    return selectList;
  }


  public async Task<IActionResult> Index()
  {
    List<Mechanic> mechanics = await _db.Mechanics.Include(m => m.VehicleMechanics).ToListAsync();
    return View(mechanics);
  }

  public ActionResult Create()
  {
    return View(new MechanicFormViewModel(MakeSelectList()));
  }

  [HttpPost]
  public async Task<IActionResult> Create(MechanicFormViewModel model)
  {
    if (!ModelState.IsValid)
    {
      model.MakeSelectList = MakeSelectList(model.SelectedMakes);
      return View(model);
    }
    Mechanic newMechanic = model.ToMechanic();
    _db.Mechanics.Add(newMechanic);
    await _db.SaveChangesAsync();
    foreach (int i in model.SelectedMakes)
    {
      MakeMechanic newMakeMechanic = new()
      {
        MakeId = i,
        MechanicId = newMechanic.MechanicId
      };
      _db.MakeMechanics.Add(newMakeMechanic);
    }
    _db.SaveChanges();
    return RedirectToAction("Details", new { id = newMechanic.MechanicId});
  }

  public async Task<IActionResult> Details(int id)
  {

    Mechanic mechanic = await _db
    .Mechanics
    .Include(m => m.VehicleMechanics)
    .ThenInclude(join => join.Vehicle)
    .Include(m => m.MakeMechanics)
    .ThenInclude(mm => mm.Make)
    .FirstOrDefaultAsync(m => m.MechanicId == id);

    List<int> approvedVehicles = mechanic.MakeMechanics.Select(mm => mm.MakeId).ToList();

    List<Vehicle> unselected = await _db.Vehicles
    .Include(v => v.VehicleMechanics)
    .Include(v => v.MakeVehicles)
    .Where(v => !v.VehicleMechanics.Any(vm => vm.MechanicId == id))
    .Where(v => approvedVehicles.Any(i => i == v.MakeVehicles.First().MakeId))
    .ToListAsync();
    
    SelectList vehiclesSelectList = new(unselected, "VehicleId", "YearMakeModelPlate");

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
    .Include(m => m.MakeMechanics)
    .FirstOrDefaultAsync(m => m.MechanicId == id);
    List<int> makeIds = thisMechanic.MakeMechanics.Select(mm => mm.MakeId).ToList();
    return View(new MechanicFormViewModel(thisMechanic, MakeSelectList(makeIds), true));
  }
  [HttpPost]
  public async Task<IActionResult> Edit(MechanicFormViewModel model)
  {
    if (!ModelState.IsValid)
    {
      model.MakeSelectList = MakeSelectList(model.SelectedMakes);
      return View(model);
    }
    Mechanic thisMechanic = await _db
    .Mechanics
    .Include(m => m.MakeMechanics)
    .FirstOrDefaultAsync(m => m.MechanicId == model.MechanicId);
    foreach (MakeMechanic mm in thisMechanic.MakeMechanics)
    {
      _db.MakeMechanics.Remove(mm);
    }
    _db.SaveChanges();
    foreach (int i in model.SelectedMakes)
    {
      MakeMechanic newMakeMechanic = new()
      {
        MakeId = i,
        MechanicId = model.MechanicId
      };
      _db.MakeMechanics.Add(newMakeMechanic);
    }
    _db.Mechanics.Update(model.ToMechanic(thisMechanic));
    await _db.SaveChangesAsync();
    return RedirectToAction("Details", new { id = model.MechanicId });
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
