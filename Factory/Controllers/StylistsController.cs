using Microsoft.AspNetCore.Mvc;
using Factory.Models;

namespace Factory.Controllers;

  public class StylistsController : Controller
  {
    private readonly FactoryContext _db;

    public StylistsController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Stylist> Stylists = _db.GetStylistList();
      ViewBag.PageTitle = $"All Stylists";
      return View(Stylists);
    }

    public ActionResult Create()
    {
      ViewBag.PageTitle = $"Add New Stylist";
      Dictionary<string, object> model = new() {
            {"Stylist", new Stylist()},
            {"Usage", "create"},
        };
      return View(model);
    }

    [HttpPost]
    public ActionResult Create(Stylist stylist)
    {
      if (ModelState.IsValid)
      {
        stylist.DateAdded = DateTime.Now;
        _db.Stylists.Add(stylist);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      ViewBag.PageTitle = $"Add New Stylist";
      Dictionary<string, object> model = new() {
            {"Stylist", stylist},
            {"Usage", "create"},
        };
      return View(model);
    }

    public ActionResult Details(int id)
    {
      Stylist targetStylist = _db.GetStylist(id);
      ViewBag.PageTitle = $"{targetStylist.Name}";
      return View(targetStylist);
    }

    public ActionResult Edit(int id)
    {
      Stylist thisStylist = _db.GetStylist(id);
      ViewBag.PageTitle = $"Editing {thisStylist.Name}";
      Dictionary<string, object> model = new() {
            {"Stylist", thisStylist},
            {"Usage", "edit"},
        };
      return View(model);
    }
    [HttpPost]
    public ActionResult Edit(Stylist stylist)
    {
      if (ModelState.IsValid) 
      {
      _db.Stylists.Update(stylist);
      _db.SaveChanges();
      return RedirectToAction("Index");
      }
      ViewBag.PageTitle = $"Editing {stylist.Name}";
      Dictionary<string, object> model = new() {
            {"Stylist", stylist},
            {"Usage", "edit"},
        };
      return View(model);
    }

    [HttpPost]
    public ActionResult Delete(int id)
    {
      Stylist targetStylist = _db.GetStylist(id);
      _db.Stylists.Remove(targetStylist);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
