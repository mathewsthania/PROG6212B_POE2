//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
using System;
using System.ComponentModel.DataAnnotations;

namespace PROG_POE2.Models
{
    public class ClaimModel
    {
        public int ClaimID { get; set; }

        public string LecturerFirstName { get; set; }

        public string LecturerLastName { get; set; }

        public int HoursWorked { get; set; }

        public decimal HourlyRate { get; set; }

        public DateTime ClaimStartDate { get; set; }

        public DateTime ClaimEndDate { get; set; }

        public decimal TotalAmount => (decimal)HoursWorked * HourlyRate;

        public string SupportingDocumentUrl { get; set; }

        public string AdditionalNotes { get; set; }

        public string Status { get; set; }

    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//