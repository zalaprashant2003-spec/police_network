using System;

namespace WebApplication1.Models
{
    public class CreateThiefViewModel
    {
        public string Name { get; set; }
        public string ThiefPlaceAsOfNow { get; set; }
        public DateTime AdmittedTime { get; set; }
        public DateTime? ReleaseTime { get; set; }
    }

}
