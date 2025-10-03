using System;

namespace WebApplication1.Models
{
    public class ThiefViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ThiefPlaceAsOfNow { get; set; }
        public DateTime AdmittedTime { get; set; }
        public DateTime? ReleaseTime { get; set; }
    }

}
