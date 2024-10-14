using Microsoft.AspNetCore.Mvc;
using PROG_POE2.Models;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Controllers
{
	public class UserController : Controller
	{
		// instantiate UserTable object to interact with the database
		public UserTable1 usrtbl = new UserTable1();

		// Action method to handle SignUp POST request
		[HttpPost]
		public ActionResult SignUp(UserTable1 Users)
		{
			// call insert_User method of UserTable to insert user data into the database
			var result = usrtbl.insert_User(Users);

			// redirecting to Home/Index action after a successful SignUp
			return RedirectToAction("Login", "Home");
		}

		// Action Method to handle SignUp GET request
		[HttpGet]
		public ActionResult SignUp()
		{
			// return the SignUp view with the UserTable object
			return View(usrtbl);
		}
	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
