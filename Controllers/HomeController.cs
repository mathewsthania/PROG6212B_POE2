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
        public async Task<IActionResult> SubmitClaim(string firstName, string surname, int hoursWorked, decimal hourlyRate, DateTime claimStartDate, DateTime claimEndDate, string notes, IFormFile supportingDocument)
        {
            if (supportingDocument != null && supportingDocument.Length > 0)
            {
                using var stream = supportingDocument.OpenReadStream();
                var fileName = Path.GetFileName(supportingDocument.FileName);

                try
                {
                    string blobUrl = await _blobService.UploadBlobAsync("supporting-documents", fileName, stream);

                    ClaimModel newClaim = new ClaimModel
                    {
                        ClaimID = claimList.Count + 1,
                        LecturerFirstName = firstName,
                        LectureLastName = surname,
                        HoursWorked = hoursWorked,
                        HourlyRate = hourlyRate,
                        ClaimStartDate = claimStartDate,
                        ClaimEndDate = claimEndDate,
                        SupportingDocumentUrl = blobUrl,
                        AdditionalNotes = notes,
                        Status = "Pending",
                    };

                    claimList.Add(newClaim);
                
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading file, please try again!");
                    ModelState.AddModelError(string.Empty, "There was an error uploading the file. Please try again.");
                }
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