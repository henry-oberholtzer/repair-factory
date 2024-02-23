using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// sourced from https://techyian.github.io/2020-03-02-aspnetcore-breadcrumbs/ //
// adapted to fit AspNetCore.Mvc by Henry Oberholtzer //
namespace Factory.Models;

public class BreadcrumbActionFilter : ActionFilterAttribute
{
  public override void OnActionExecuted(ActionExecutedContext filterContext)
  {
    List<Breadcrumb> breadcrumbs = ConfigureBreadcrumb(filterContext);

    Controller controller = filterContext.Controller as Controller;
    controller.ViewBag.Breadcrumbs = breadcrumbs;

    base.OnActionExecuted(filterContext);
  }

  private static List<Breadcrumb> ConfigureBreadcrumb(ActionExecutedContext context)
  {
    List<Breadcrumb> breadcrumbList = new();
    string homeControllerName = "Home";

    breadcrumbList.Add(new Breadcrumb
    {
      Text = "Home",
      Action = "Index",
      Controller = homeControllerName,
      Active = true
    });

    if (context.HttpContext.Request.Path.HasValue)
    {
      string[] pathSplit = context.HttpContext.Request.Path.Value.Split("/");
      for (int i = 0; i < pathSplit.Length; i++)
      {
        if (string.IsNullOrEmpty(pathSplit[i]) || string.Compare(pathSplit[i], homeControllerName, true) == 0)
        {
          continue;
        }

        Type controller = GetControllerType(pathSplit[i] + "Controller");

        if (controller != null)
        {
          var indexMethod = controller.GetMethod("Index");

          if (indexMethod != null)
          {
            breadcrumbList.Add(new Breadcrumb
            {
              Text = CamelCaseSpacing(pathSplit[i]),
              Action = "Index",
              Controller = pathSplit[i],
              Active = true
            });

            if (i + 1 < pathSplit.Length && string.Compare(pathSplit[i + 1], "Index", true) == 0)
            {
              breadcrumbList.LastOrDefault().Active = false;

              return breadcrumbList;
            }
          }
        }

        if (i - 1 > 0)
        {
          var prevController = GetControllerType(pathSplit[i - 1] + "Controller");
          if (prevController != null)
          {
            MethodInfo method = prevController.GetMethod(pathSplit[i]);

            if (method != null)
            {
              breadcrumbList.Add(new Breadcrumb
              {
                Text = CamelCaseSpacing(pathSplit[i]),
                Action = pathSplit[i],
                Controller = pathSplit[i - 1],
                Active = true
              });
            }
          }
        }
      }
    }
    breadcrumbList.LastOrDefault().Active = false;
    return breadcrumbList;
  }

  private static Type GetControllerType(string name)
  {
    Type controller = null;
    try
    {
      controller = Assembly.GetCallingAssembly().GetType("WebApp.Web.Controllers." + name);
    }
    catch
    { }

    return controller;
  }

  private static string CamelCaseSpacing(string s)
  {
    var r = new Regex(@"
        (?<=[A-Z])(?=[A-Z][a-z]) |
        (?<=[^A-Z])(?=[A-Z]) |
        (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

    return r.Replace(s, " ");
  }

    public void OnActionExecuting(ActionContext context)
    {
        throw new NotImplementedException();
    }
}

public class Breadcrumb
{
  public string Text { get; set; }
  public string Action { get; set; }

  public string Controller { get; set; }

  public bool Active { get; set; }

  public Breadcrumb() {}

  public Breadcrumb(string text, string action, string controller, bool active)
  {
    Text = text;
    Action = action;
    Controller = controller;
    Active = active;
  }
}
