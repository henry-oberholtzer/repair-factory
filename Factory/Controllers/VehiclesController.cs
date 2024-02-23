using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Factory.Models;
using Factory.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Factory.Controllers;

[BreadcrumbActionFilter]
public class VehiclesController : Controller
{
    private readonly FactoryContext _db;

    public VehiclesController(FactoryContext db)
    {
        _db = db;
    }

    public Dictionary<string, object> FormModel(VehicleViewModel vehicleViewModel, SelectList makeSelectList, string action)
    {
        return new Dictionary<string, object> {
            {"VehicleViewModel", vehicleViewModel},
            {"makeSelectList", makeSelectList},
            {"Action", action}
        };
    }

    public VehicleViewModel VehicleViewModelMap(Vehicle v)
    {
        return new VehicleViewModel
        {
            VehicleId = v.VehicleId,
            LicensePlate = v.LicensePlate,
            MakeId = v.MakeVehicles.First().MakeId,
            Model = v.Model,
            ModelYear = v.ModelYear,
            Color = v.Color,
            Condition = v.Condition
        };
    }

    public Vehicle VehicleViewModelMap(VehicleViewModel vehicleViewModel, Make make)
    {
        return new Vehicle
        {
            VehicleId = vehicleViewModel.VehicleId,
            LicensePlate = vehicleViewModel.LicensePlate,
            Model = vehicleViewModel.Model,
            ModelYear = vehicleViewModel.ModelYear,
            Color = vehicleViewModel.Color,
            Condition = vehicleViewModel.Condition,
            YearMakeModelPlate = $"{vehicleViewModel.ModelYear} {make.Name} {vehicleViewModel.Model} | {vehicleViewModel.LicensePlate}",
            DateAdded = DateTime.Now
        };
    }

    public Vehicle VehicleViewModelMap(VehicleViewModel vehicleViewModel, Vehicle vehicle, Make make)
    {
        vehicle.LicensePlate = vehicleViewModel.LicensePlate;
        vehicle.Model = vehicleViewModel.Model;
        vehicle.ModelYear = vehicleViewModel.ModelYear;
        vehicle.Color = vehicleViewModel.Color;
        vehicle.Condition = vehicleViewModel.Condition;
        vehicle.YearMakeModelPlate = $"{vehicleViewModel.ModelYear} {make.Name} {vehicleViewModel.Model} | {vehicleViewModel.LicensePlate}";
        return vehicle;
    }
    public async Task<IActionResult> Index()
    {
        List<Vehicle> vehicles = await _db.Vehicles.Include(v => v.VehicleMechanics).ToListAsync();
        return View(vehicles);
    }

    public async Task<IActionResult> Create()
    {
        List<Make> makeList = await _db.Makes.OrderBy(m => m.Name).ToListAsync();
        SelectList makeSelectList = new(makeList, "MakeId", "Name");
        return View(FormModel(new VehicleViewModel(), makeSelectList, "Create"));
    }


    [HttpPost]
    public async Task<IActionResult> Create(VehicleViewModel vehicleViewModel)
    {
        if (!ModelState.IsValid)
        {
            List<Make> makeList = await _db.Makes.OrderBy(m => m.Name).ToListAsync();
            SelectList makeSelectList = new(makeList, "MakeId", "Name");
            Dictionary<string, object> model = FormModel(vehicleViewModel, makeSelectList, "Create");
            return View(model);
        }
        Make makeReference = new();
        if (!string.IsNullOrEmpty(vehicleViewModel.NewMake))
        {
#nullable enable // check and see if NewMake exists & does not match anything in the DB
            Make? existing = await _db.Makes.FirstOrDefaultAsync(m => m.Name == vehicleViewModel.NewMake);
#nullable disable
            if (existing == null)
            {
                // if it does not match, add a new Make, and get the makes Id
                Make newMake = new()
                {
                    Name = vehicleViewModel.NewMake
                };
                _db.Makes.Add(newMake);
                await _db.SaveChangesAsync();
                vehicleViewModel.MakeId = newMake.MakeId;
                makeReference = newMake;
            }
            else
            {
                // if it does match something in the database, get the match Id
                vehicleViewModel.MakeId = existing.MakeId;
                makeReference = existing;
            }
        }

        // create a new vehicle
        Vehicle newVehicle = VehicleViewModelMap(vehicleViewModel, makeReference);
        _db.Vehicles.Add(newVehicle);
        await _db.SaveChangesAsync();
        // add new MakeVehicle
        MakeVehicle makeVehicle = new()
        {
            MakeId = vehicleViewModel.MakeId,
            VehicleId = newVehicle.VehicleId
        };
        _db.MakeVehicles.Add(makeVehicle);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Edit(int id)
    {
        Vehicle v = await _db
        .Vehicles
        .Include(v => v.MakeVehicles)
        .FirstOrDefaultAsync(vehicle => vehicle.VehicleId == id);
        List<Make> makeList = await _db.Makes.OrderBy(m => m.Name).ToListAsync();
        SelectList makeSelectList = new(makeList, "MakeId", "Name");
        foreach (SelectListItem item in makeSelectList)
        {
            if (item.Value == v.MakeVehicles.First().MakeId.ToString())
            {
                item.Selected = true;
            }
        }
        return View(FormModel(VehicleViewModelMap(v), makeSelectList, "Edit"));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(VehicleViewModel vehicleViewModel)
    {
        if (!ModelState.IsValid)
        {
            List<Make> makeList = await _db.Makes.OrderBy(m => m.Name).ToListAsync();
            SelectList makeSelectList = new(makeList, "MakeId", "Name");
            foreach (SelectListItem item in makeSelectList)
            {
                if (item.Value == vehicleViewModel.MakeId.ToString())
                {
                    item.Selected = true;
                }
            }
            return View(FormModel(vehicleViewModel, makeSelectList, "Edit"));
        }
        Vehicle vehicleToEdit = await _db.Vehicles
        .Include(v => v.MakeVehicles)
        .ThenInclude(mv => mv.Make)
        .FirstOrDefaultAsync(vehicle => vehicle.VehicleId == vehicleViewModel.VehicleId);

        Make makeReference = vehicleToEdit.MakeVehicles.First().Make;


        if (!string.IsNullOrEmpty(vehicleViewModel.NewMake))
        {
#nullable enable // check and see if NewMake exists & does not match anything in the DB
            Make? existing = await _db.Makes.FirstOrDefaultAsync(m => m.Name == vehicleViewModel.NewMake);
#nullable disable
            if (existing == null)
            {
                // if it does not match, add a new Make, and get the makes Id
                Make newMake = new()
                {
                    Name = vehicleViewModel.NewMake
                };
                _db.Makes.Add(newMake);
                await _db.SaveChangesAsync();
                vehicleViewModel.MakeId = newMake.MakeId;
                makeReference = newMake;
            }
            else
            {
                // if it does match something in the database, get the match Id
                vehicleViewModel.MakeId = existing.MakeId;
                makeReference = existing;
            }

        }
        else if (vehicleViewModel.MakeId != vehicleToEdit.MakeVehicles.First().MakeId && vehicleViewModel.MakeId != 0)
        {
#nullable enable // check and see if NewMake exists & does not match anything in the DB
            Make? existing = await _db.Makes.FirstOrDefaultAsync(m => m.MakeId == vehicleViewModel.MakeId);
#nullable disable
            if (existing != null)
            {
                makeReference = existing;
            }
        }
        if (vehicleViewModel.MakeId != 0)
        {
            IQueryable<MakeVehicle> mvList = _db.MakeVehicles.Where(mv => mv.VehicleId == vehicleToEdit.VehicleId);
            foreach (MakeVehicle mV in mvList)
            {
                _db.MakeVehicles.Remove(mV);
            }
            await _db.SaveChangesAsync();
            MakeVehicle makeVehicle = new()
            {
                MakeId = vehicleViewModel.MakeId,
                VehicleId = vehicleToEdit.VehicleId
            };
            _db.MakeVehicles.Add(makeVehicle);
            await _db.SaveChangesAsync();
        }

        _db.Vehicles.Update(VehicleViewModelMap(vehicleViewModel, vehicleToEdit, makeReference));
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", new { id = vehicleToEdit.VehicleId });

    }

    public async Task<IActionResult> Details(int id)
    {
        Vehicle vehicle = await _db
        .Vehicles
        .Include(v => v.VehicleMechanics)
        .ThenInclude(join => join.Mechanic)
        .Include(v => v.MakeVehicles)
        .FirstOrDefaultAsync(v => v.VehicleId == id);

        List<Mechanic> unselected = await _db.Mechanics
        .Include(m => m.VehicleMechanics)
        .Include(m => m.MakeMechanics)
        .Where(m => !m.VehicleMechanics.Any(vm => vm.VehicleId == id))
        .Where(m => m.MakeMechanics.Any(i => i.MakeId == vehicle.MakeVehicles.First().MakeId))
        .ToListAsync();
        SelectList mechanicsSelectList = new(unselected, "MechanicId", "LastName");


        Dictionary<string, object> model = new() {
            {"selectList", mechanicsSelectList},
            {"joinEntity", new VehicleMechanic()},
            {"vehicle", vehicle}
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Vehicle vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
        _db.Vehicles.Remove(vehicle);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AssignMechanic(VehicleMechanic vm)
    {
#nullable enable
        VehicleMechanic? joinEntity = await _db.VehicleMechanics
        .FirstOrDefaultAsync(join => join.VehicleId == vm.VehicleId && join.MechanicId == vm.MechanicId);
#nullable disable
        if (joinEntity == null && vm.VehicleId != 0)
        {
            _db.VehicleMechanics.Add(vm);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = vm.VehicleId });
        }
        return RedirectToAction("Details", new { id = vm.VehicleId });
    }

    [HttpPost]
    public async Task<IActionResult> UnassignMechanic(int id)
    {
        VehicleMechanic joinEntity = await _db.VehicleMechanics.FirstOrDefaultAsync(vm => vm.VehicleMechanicId == id);
        int vehicleId = joinEntity.VehicleId;
        _db.VehicleMechanics.Remove(joinEntity);
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", new { id = vehicleId });
    }
}
