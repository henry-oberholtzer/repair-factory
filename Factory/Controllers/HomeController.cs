using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using Microsoft.EntityFrameworkCore;

namespace Factory.Controllers
{
  public class HomeController : Controller
  {

    private readonly FactoryContext _db;

    public HomeController(FactoryContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
      List<Vehicle> vehicles = await _db.Vehicles.Include(v => v.VehicleMechanics).ToListAsync();
      List<Mechanic> mechanics = await _db.Mechanics.Include(v => v.VehicleMechanics).ToListAsync();
      Dictionary<string, object> model = new() {
        {"vehicles", vehicles},
        {"mechanics", mechanics}
      };
      return View(model);
    }

  }
}
