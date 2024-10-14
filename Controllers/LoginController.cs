using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Claims;
using PROG_POE2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Controllers
{
	public class LoginController : Controller
	{
		// creating an instance of the UserTable class, to access data - database
		public UserTable1 usrtbl = new UserTable1();

		// Action method - handling the default index view
		public IActionResult Index()
		{
			return View();
		}

		// creating a LoginModel instance - for managing the login operations
		private readonly LoginModel login;

		// creating a constructor for the LoginController class
		public LoginController()
		{
			login = new LoginModel();
		}

		// Action method for handling POST requests for user privacy
		[HttpPost]
		public async Task<IActionResult> UserLogin(UserTable1 model)
		{

			if (ModelState.IsValid)
			{
				return View("Login", model); ;
			}

			// creating a new instance of the LoginModel
			var loginModel = new LoginModel();

			// using the object created(loginModel) to call the SelectUser method to find the user by Name,Email & Password
			int UserID = login.SelectUser(model.Name, model.Email, model.Password);

			// checking if the UserID is valid (if its not equal to -1)
			if (UserID != -1)

			{
				// User found, proceed with login logic (e.g., set authentication cookie)
				// Redirects to MyWorkPage(HomeController) - now with the UsersID

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, model.Name),
					new Claim(ClaimTypes.Email, model.Email),
					new Claim(ClaimTypes.NameIdentifier, UserID.ToString())
				};

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

				HttpContext.Session.SetString("UserName", model.Name);

				return RedirectToAction("MyWorkPageShopping", "Home", new { UserID = UserID });
			}

			else
			{
				ViewBag.ErrorMessage = "Email or Password entered is incorrect, Please try again!";
				// User not found, handle accordingly (e.g., show error message)

				return View("Login", model);
			}
		}

		[HttpGet]
		public IActionResult Login()
		{
			// Returns the login view page
			return View("Login");
		}

	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
