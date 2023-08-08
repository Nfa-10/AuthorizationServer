using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuthorizationServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}