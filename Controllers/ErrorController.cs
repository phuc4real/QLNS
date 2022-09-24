using System.Web.Mvc;

namespace QLNS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("Error");
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}