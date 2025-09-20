namespace WebApplication1.Models
{
    public class Msg
    {
        public int Id { get; set; }
        public string sender { get; set; }
        public string message { get; set; }
        public string reciever {get;set;}
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
