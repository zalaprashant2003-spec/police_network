using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FIRCreateViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [Required]
        public string Address { get; set; }

        public string ComplaintType { get; set; }
        public string Description { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Status { get; set; }

        public List<string> SelectedThieves { get; set; } = new List<string>();
        public List<SelectListItem> Thieves { get; set; } = new List<SelectListItem>();
    }
}
