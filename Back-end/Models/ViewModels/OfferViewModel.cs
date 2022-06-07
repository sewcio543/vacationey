using Backend.Models.DbModels;

namespace Backend.Models.ViewModels
{
    public class OfferViewModel
    {
        public Offer? Offer { get; set; }
        public Country? Country { get; set; }
        public City? City { get; set; }

        public Hotel? Hotel { get; set; }

        public string? countryString { get; set; }
    }
}
