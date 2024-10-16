using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROG_POE2.Models;
using PROG_POE2.Services;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlobService _blobService;  
        private static List<ClaimModel> claimList = new List<ClaimModel>();

        public HomeController(ILogger<HomeController> logger, BlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult ApproveClaim()
        {
            return View(claimList);
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
            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                const long maxFileSize = 5242880;
                if (SupportingDocument.Length > maxFileSize)
                {
                    TempData["ErrorMessage"] = "The file you are trying to submit must be less than 5 MB!";
                    return RedirectToAction("SubmitClaim");
                }

                using var stream = SupportingDocument.OpenReadStream();
                var fileName = Path.GetFileName(SupportingDocument.FileName);

                try
                {
                    string blobUrl = await _blobService.UploadBlobAsync("supporting-documents", fileName, stream);

                    ClaimModel newClaim = new ClaimModel
                    {
                        ClaimID = claimList.Count + 1,
                        LecturerFirstName = LecturerFirstName,
                        LecturerLastName = LecturerLastName,
                        HoursWorked = HoursWorked,
                        HourlyRate = HourlyRate,
                        ClaimStartDate = ClaimStartDate,
                        ClaimEndDate = ClaimEndDate,
                        SupportingDocumentUrl = blobUrl,
                        AdditionalNotes = AdditionalNotes,
                        Status = "Pending",
                    };

                    claimList.Add(newClaim);

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

            return RedirectToAction("SubmitClaim");
        }


        // processing the claim - method
        [HttpPost]
        public IActionResult ApproveRejectClaims(int ClaimId, string action)
        {
            var claim = claimList.FirstOrDefault(c => c.ClaimID == ClaimId);
            if (claim == null)
            {
                if (action == "approve")
                {
                    claim.Status = "Approved";
                }
                else if (action == "reject")
                {
                    claim.Status = "Rejected";
                }

                claimList.Remove(claim);
            }

            return RedirectToAction("ApproveClaim");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//