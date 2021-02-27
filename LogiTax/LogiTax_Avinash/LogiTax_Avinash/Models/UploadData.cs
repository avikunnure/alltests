namespace LogiTax_Avinash.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UploadData")]
    public partial class UploadData
    {
        public Guid Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string GSTIN { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Range(0, 999999999999, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal? TurnoverAmount { get; set; }

        [EmailAddress(ErrorMessage ="Please enter valid email address")]
        [Required( ErrorMessage ="please enter email")]
        public string ContactEmail { get; set; }

        [MaxLength(10,ErrorMessage ="Please enter valid mobile no")]
        [MinLength(10, ErrorMessage = "Please enter valid mobile no")]
        [DataType(DataType.PhoneNumber,ErrorMessage = "Please enter valid mobile no")]
        public string ContactNumber { get; set; }
    }


}
