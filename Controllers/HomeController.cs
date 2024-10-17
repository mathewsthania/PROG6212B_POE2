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
        
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(string LecturerFirstName, string LecturerLastName, int HoursWorked, decimal HourlyRate, DateTime ClaimStartDate, DateTime ClaimEndDate, string AdditionalNotes, IFormFile SupportingDocument)
        {
            // defining the maximum file size
            const long maxFileSize = 5 * 1024 * 1024;

            // checking the file that the lecturer uploads
            if (SupportingDocument == null && SupportingDocument.Length == 0)
            {
                TempData["ErrorMessage"] = "Please upload a supporting document!";
                return RedirectToAction("SubmitClaim");
             }


            // checking the file size of the supporting document
			if (SupportingDocument.Length > maxFileSize)
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

            using var stream = SupportingDocument.OpenReadStream();
            var fileName = Path.GetFileName(SupportingDocument.FileName);
				
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
                        SupportingDocumentUrl = "",
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
                    _logger.LogError(ex, "Error uploading file, please try again!");
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
        public IActionResult LecturerClaimStatus(string lecturerFirstName, string lecturerLastName)
        {
            var lecturerClaims = _context.Claims
                .Where(c => c.LecturerFirstName == lecturerFirstName && c.LecturerLastName == lecturerLastName).ToList();
            return View(lecturerClaims);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//