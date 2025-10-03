namespace WebApplication1.Models
{
    public class FIRThief
    {
        public string FIRId { get; set; }
        public string ThiefId { get; set; }
        public FIR FIR { get; set; }
        public Thief Thief { get; set; }
    }

}
