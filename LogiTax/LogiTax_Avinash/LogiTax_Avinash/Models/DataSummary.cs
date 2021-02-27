using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogiTax_Avinash.Models
{
    public class DataSummary
    {
        public UploadData UploadData { get; set; }
        public bool IsValid { get; set; }
        public bool IsErrorData { get { return results.Count > 0; } }
        public bool IsDuplicateData { get; set; }
        public List<ValidationResult> results {get ;set   ;}

        public DataSummary()
        {
            results = new List<ValidationResult>();
            UploadData = new UploadData();
        }
    }
}