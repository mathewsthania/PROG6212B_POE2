using System.Diagnostics;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using PROG_POE2.Data;
using PROG_POE2.Models;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _context;

		public HomeController(ILogger<HomeController> logger, AppDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}


		public IActionResult ApproveClaim()
		{
			var pendingClaims = _context.Claims.Where(c => c.Status == "Pending").ToList();
			return View(pendingClaims);
		}


		public IActionResult SubmitClaim()
		{
			return View();
		}


		public IActionResult Claims1()
		{
			var userName = User.Identity.Name;

			var claims = _context.Claims
				.Where(c => c.LecturerFirstName == userName)
				.ToList();

			return View(claims);
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

		[HttpPost]
		public async Task<IActionResult> SubmitClaim(string LecturerFirstName, string LecturerLastName, int HoursWorked, decimal HourlyRate, DateTime ClaimStartDate, DateTime ClaimEndDate, string? AdditionalNotes, IFormFile? SupportingDocument)
		{
			// adding in exception handling
			if (string.IsNullOrWhiteSpace(LecturerFirstName) || string.IsNullOrWhiteSpace(LecturerLastName))
			{
				TempData["ErrorMessage"] = "Please fill in your first name and last name!";
				return RedirectToAction("SubmitClaim");
			}

			if (HoursWorked <= 0 || HourlyRate <= 0)
			{
				TempData["ErrorMessage"] = "Hours worked/ Hourly Rate must be greater than zero!";
				return RedirectToAction("SubmitClaim");
			}

			if (ClaimEndDate < ClaimStartDate)
			{
				TempData["ErrorMessage"] = "Claim end date cannot be before the claim start date, please try again!";
				return RedirectToAction("SubmitClaim");
			}

			// defining the maximum file size
			const long maxFileSize = 5 * 1024 * 1024;

			// checking the file that the lecturer uploads
			if (SupportingDocument != null && SupportingDocument.Length > 0)
			{
				TempData["ErrorMessage"] = "Please upload a supporting document!";
				return RedirectToAction("SubmitClaim");
			}


			// checking the file size of the supporting document
			if (SupportingDocument != null && SupportingDocument.Length > maxFileSize)
			{
				TempData["ErrorMessage"] = "The file must be less than 5 MB!";
				return RedirectToAction("SubmitClaim");
			}

			// checking for the file type 
			var allowedFileTypes = new[] { ".pdf", ".docx", ".xlsx" };
			var fileExtension = Path.GetExtension(SupportingDocument.FileName).ToLower();

			if (!allowedFileTypes.Contains(fileExtension))
			{
				TempData["ErrorMessage"] = "The file you are trying to upload is not allowed, only .pdf, .docx, and .xlsx file types are allowed.";
				return RedirectToAction("SubmitClaim");
			}

			using var stream = new MemoryStream();
			await SupportingDocument.CopyToAsync(stream);
			var fileBytes = stream.ToArray();

			try
			{
				ClaimModel newClaim = new ClaimModel
				{
					LecturerFirstName = LecturerFirstName,
					LecturerLastName = LecturerLastName,
					HoursWorked = HoursWorked,
					HourlyRate = HourlyRate,
					ClaimStartDate = ClaimStartDate,
					ClaimEndDate = ClaimEndDate,
					SupportingDocument = fileBytes,
					SupportingDocumentExtension = fileExtension,
					AdditionalNotes = AdditionalNotes,
					Status = "Pending",
					TotalAmount = HoursWorked * HourlyRate
				};

				// saving the claim to the database
				_context.Claims.Add(newClaim);
				await _context.SaveChangesAsync();

				TempData["SuccessMessage"] = "Your claim was submitted successfully!";
				return RedirectToAction("SubmitClaim");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error submmiting your claim: {Message}", ex.Message);

				TempData["ErrorMessage"] = "Error submitting your claim! Please try again.";
				return RedirectToAction("SubmitClaim");
			}
		}


		// processing the claim - method
		[HttpPost]
		public async Task<IActionResult> ApproveRejectClaims(int ClaimId, string action)
		{
			var claim = await _context.Claims.FindAsync(ClaimId);
			if (claim != null)
			{
				if (action == "approve")
				{
					claim.Status = "Approved";
				}
				else if (action == "reject")
				{
					claim.Status = "Rejected";
				}

				_context.Claims.Update(claim);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("ApproveClaim");
		}

		// method for the status of the claim
		[HttpGet]
		public IActionResult LecturerClaimStatus()
		{
			string lecturerFirstName = HttpContext.Session.GetString("LecturerFirstName");
			string lecturerLastName = HttpContext.Session.GetString("LecturerLastName");

			// Check if session variables are set
			if (string.IsNullOrEmpty(lecturerFirstName) || string.IsNullOrEmpty(lecturerLastName))
			{
				TempData["ErrorMessage"] = "User session data not found.";
				return RedirectToAction("Index", "Home"); // Redirect if not found
			}

			// Retrieving claims for the logged-in lecturer
			var lecturerClaims = _context.Claims
				.Where(c => c.LecturerFirstName == lecturerFirstName && c.LecturerLastName == lecturerLastName)
				.OrderByDescending(c => c.DateSubmitted)
				.ToList();

			return View(lecturerClaims);
		}

		[HttpGet]
		public IActionResult DownloadDocument(int id)
		{
			var claim = _context.Claims.FirstOrDefault(c => c.ClaimID == id);

			if (claim == null || claim.SupportingDocument == null)
			{
				TempData["ErrorMessage"] = "No document available.";
				return RedirectToAction("ApproveClaim");
			}

			// Set the content type
			string contentType = claim.SupportingDocumentExtension switch
			{
				".pdf" => "application/pdf",
				".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				_ => "application/octet-stream" // default if unknown extension
			};

			return File(claim.SupportingDocument, contentType, $"Document_{claim.ClaimID}{claim.SupportingDocumentExtension}");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}


//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//