using Microsoft.AspNetCore.Mvc;
using PROG_POE2.Models;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Controllers
{
	public class UserController : Controller
	{
		// instantiate UserTable object to interact with the database
		public LecturerTable lectTbl = new LecturerTable();
		public ProgrammeCoordinatorAcademicManagerTable progAcdmTbl = new ProgrammeCoordinatorAcademicManagerTable();

		// Action method to handle SignUp POST request
		[HttpPost]
		public ActionResult SignUp(string role, string name, string surname, string email, string password)
		{
			if (role == "Lecturer")
			{
				var lecturer = new LecturerTable
				{
					Name = name,
					Surname = surname,
					Email = email,
					Password = password
				};

				var result = lectTbl.insert_Lecturer(lecturer);
			}
			else if (role == "Programme Coordinator/Academic Manager")
			{
				var programcoordinatoracademicmanager = new ProgrammeCoordinatorAcademicManagerTable
				{
					Name = name,
					Surname = surname,
					Email = email,
					Password = password
				};

				var result = progAcdmTbl.insert_ProgrammeCoordinatorAcademicManager(programcoordinatoracademicmanager);
			}

			// redirecting to Home/Index action after a successful SignUp
			return RedirectToAction("Login", "Home");
		}

		// Action Method to handle SignUp GET request
		[HttpGet]
		public ActionResult SignUp()
		{
			// return the SignUp view with the UserTable object
			return View();
		}
	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
