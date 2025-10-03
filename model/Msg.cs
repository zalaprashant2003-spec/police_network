using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Msg
    {
        public int Id { get; set; }
        [Required]
        public string sender { get; set; }
        [Required]
        public string message { get; set; }
        [Required]
        public string reciever {get;set;}
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
