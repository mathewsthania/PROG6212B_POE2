using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROG_POE2.Models;

namespace PROG_POE2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApproveClaim()
        {
            return View();
        }

		public IActionResult SubmitClaim()
		{
			return View();
		}

        public IActionResult Claims1()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
