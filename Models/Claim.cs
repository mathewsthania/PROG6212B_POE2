//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
using System;
using System.ComponentModel.DataAnnotations;

namespace PROG_POE2.Models
{
    public class Claim
    {
        public int ClaimID { get; set; }

        public double HoursWorked { get; set; }

        public decimal HourlyRate { get; set; }

        public decimal TotalAmount => (decimal)HoursWorked * HourlyRate;

        public string ClaimMonth { get; set; }

        public string Status { get; set; } = "Pending";

        public string SupportingDocumentPath { get; set; }

    }
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//