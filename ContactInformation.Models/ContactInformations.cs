using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ContactInformation.Models
{
    public class ContactInformations
	{
		
		[Key]	
		public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }      
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
		public DateTime? Created { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? Modified { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
