using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class FIRAnalysisViewModel
    {
        public int TotalFIRs { get; set; }
        public Dictionary<string, int> ByStatus { get; set; }
        public Dictionary<string, int> ByComplaintType { get; set; }
        public Dictionary<string, int> ByGender { get; set; }
    }

}
