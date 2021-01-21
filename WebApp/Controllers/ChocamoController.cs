using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers
{
    public class ChocamoController : Controller
    {
        [Route("/Chocamo")]
        public ActionResult Index() 
        {
            return View("Chocamo");
        }
    }
}