using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Thief
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ThiefPlaceAsOfNow { get; set; }
        public DateTime AdmittedTime { get; set; }
        public DateTime? ReleaseTime { get; set; }

        public List<FIRThief> FIRThieves { get; set; }
    }

}
