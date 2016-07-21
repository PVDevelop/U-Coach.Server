using System.Web.Mvc;

namespace MvcAuthrorization.Controllers
{
    public class ResourceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}