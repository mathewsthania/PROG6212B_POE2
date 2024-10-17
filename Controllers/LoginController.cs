using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Claims;
using PROG_POE2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Controllers
{
	public class LoginController : Controller
	{
		// creating an instance of the Lecturer and ProgAcademic class, to access data - database
		public LecturerTable lectTbl = new LecturerTable();
		public ProgrammeCoordinatorAcademicManagerTable progAcdmTbl = new ProgrammeCoordinatorAcademicManagerTable();

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
		public async Task<IActionResult> UserLogin(string name, string email, string password)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				ViewBag.ErrorMessage = "Please fill in all the fields!";
				ViewBag.Name = name;
				ViewBag.Email = email;
				return View("Login");
			}

			// checking for lecturer
			int lecturerUserID = login.SelectLecturer(name, email, password);

			// checking for the programme coordinator/ admin
			int programmeCoordAcadManagerUserID = login.SelectProgrammeCoordinatorAcademicManager(name, email, password);


			if (lecturerUserID != -1)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, name),
					new Claim(ClaimTypes.Email, email),
					new Claim(ClaimTypes.NameIdentifier, lecturerUserID.ToString()),
					new Claim(ClaimTypes.Role, "Lecturer")
				};


				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

				HttpContext.Session.SetString("UserName", name);
				HttpContext.Session.SetString("UserRole", "Lecturer");

				return RedirectToAction("SubmitClaim", "Home", new { UserID = lecturerUserID });
			}

			else if (programmeCoordAcadManagerUserID != -1)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, name),
					new Claim(ClaimTypes.Email, email),
					new Claim(ClaimTypes.NameIdentifier, programmeCoordAcadManagerUserID.ToString()),
					new Claim(ClaimTypes.Role, "Programme Coordinator/Academic Manager")

				};

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

				HttpContext.Session.SetString("UserName", name);
				HttpContext.Session.SetString("UserRole", "Programme Coordinator/Academic Manager");

				return RedirectToAction("ApproveClaim", "Home", new { UserID = programmeCoordAcadManagerUserID });
			}

			else
			{
				ViewBag.ErrorMessage = "Email or Password entered is incorrect, Please try again!";
				// User not found, handle accordingly (e.g., show error message)
				ViewBag.Name = name;
				ViewBag.Email = email;
				return View("Login");
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
