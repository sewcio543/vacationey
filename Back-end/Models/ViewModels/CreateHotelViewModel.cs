namespace Backend.Models.ViewModels
{
    public class CreateHotelViewModel
    {
        public string? HotelId { get; set; }
        public string? Name { get; set; }
        public int Rate { get; set; }
        public string? City { get; set; }
        public bool WiFi { get; set; }
        public bool Pool { get; set; }
    }
}
