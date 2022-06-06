using Backend.Models.DbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backend.Models.ViewModels
{
    public class CountryOfferViewModel
    {
        public List<Offer>? Offers { get; set; }
        public SelectList? Countries { get; set; }

        public string? OfferCountry { get; set; }
        public string? SearchString { get; set; }
    }
}
