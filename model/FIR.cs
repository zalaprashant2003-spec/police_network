using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FIR
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name should be between 2 and 100 characters")]
        public string Name { get; set; }

        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid contact number")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address can be maximum 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Complaint type is required")]
        [StringLength(50, ErrorMessage = "Complaint type can be maximum 50 characters")]
        public string ComplaintType { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description can be maximum 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Incident date is required")]
        [DataType(DataType.DateTime)]
        public DateTime IncidentDate { get; set; }

        public DateTime FiledDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public List<FIRThief> FIRThieves { get; set; } = new List<FIRThief>();
    }
}
