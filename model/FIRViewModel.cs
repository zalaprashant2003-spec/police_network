using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class FIRViewModel
    {
        public string Id { get; set; }

        // New complainant fields
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }

        // Existing FIR fields
        public string ComplaintType { get; set; }
        public string Description { get; set; }
        public DateTime IncidentDate { get; set; }
        public DateTime FiledDate { get; set; }
        public string Status { get; set; }

        // Thieves list
        public List<string> Thieves { get; set; } = new List<string>();
    }
}
