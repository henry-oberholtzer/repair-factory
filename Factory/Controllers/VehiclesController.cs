using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Factory.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Controllers;

public class VehiclesController : Controller
{
    private readonly FactoryContext _db;

    public VehiclesController(FactoryContext db)
    {
        _db = db;
    }

    public static Dictionary<string, object> FormModel(Vehicle vehicle, string action)
    {
        return new Dictionary<string, object> {
            {"Vehicle", vehicle},
            {"Action", action}
        };
    }
    public async Task<IActionResult> Index()
    {
        List<Vehicle> vehicles = await _db.Vehicles.Include(v => v.VehicleMechanics).ToListAsync();
        return View(vehicles);
    }

    public ActionResult Create()
    {
        return View(FormModel(new Vehicle(), "Create"));
    }

    [HttpPost]
    public ActionResult Create(Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            vehicle.DateAdded = DateTime.Now;
            _db.Vehicles.Add(vehicle);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(FormModel(vehicle, "Create"));
    }

    public async Task<IActionResult> Edit(int id)
    {
        Vehicle thisVehicle = await _db
        .Vehicles
        .FirstOrDefaultAsync(vehicle => vehicle.VehicleId == id);
        return View(FormModel(thisVehicle, "Edit"));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(FormModel(vehicle, "Edit"));
    }

    public async Task<IActionResult> Details(int id)
    {
        Vehicle thisVehicle = await _db
        .Vehicles
        .Include(v => v.VehicleMechanics)
        .ThenInclude(join => join.Mechanic)
        .FirstOrDefaultAsync(v => v.VehicleId == id);
        return View(thisVehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Vehicle vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
        _db.Vehicles.Remove(vehicle);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> AddMechanic(int vehicleId)
    {
        SelectList mechanicsSelectList = new(_db.Mechanics, "MechanicId", "LastName");
        Vehicle vehicle = await _db.Vehicles
        .Include(v => v.VehicleMechanics)
        .ThenInclude(join => join.Mechanic)
        .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        Dictionary<string, object> model = new() {
            {"selectList", mechanicsSelectList},
            {"vehicle", vehicle}
        };
        return View("_AssignedMechanics", model);
    }
    // [HttpPost]
    // public async Task<IActionResult> AddMechanic(int mechanicId) {

    //     return RedirectToAction("Details", "Vehicle", new { id = vehicle.VehicleId });
    // };
}
