using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Factory.Models;
public class FactoryContext : DbContext
{
  public DbSet<Stylist> Stylists { get; set; }
  public DbSet<Client> Clients { get; set; }
  public FactoryContext(DbContextOptions options) : base(options) { }

  public SelectList StylistSelectList(int? id = null)
  {
    SelectList stylistList = new(Stylists, "StylistId", "Name");
    if (id != null)
    {
      foreach (SelectListItem stylist in stylistList)
      {
        if (stylist.Value == id.ToString())
        {
          stylist.Selected = true;
        }
      }
    }
    return stylistList;
  }

  private IQueryable<Stylist> StylistsWithClients() => Stylists.Include(stylist => stylist.Clients);
  public Stylist GetStylist(int id) => StylistsWithClients().FirstOrDefault(stylist => stylist.StylistId == id);
  public List<Stylist> GetStylistList() => StylistsWithClients().ToList();

  private IQueryable<Client> ClientsWithStylist() => Clients.Include(client => client.Stylist);
  public Client GetClient(int id) => ClientsWithStylist().FirstOrDefault(client => client.ClientId == id);
  public List<Client> GetClientList() => ClientsWithStylist().ToList();
}
