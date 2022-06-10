using System.ComponentModel.DataAnnotations;

namespace Backend.Models.ViewModels
{
    public class CreateOfferViewModel
    {
        public int OfferId { get; set; }
        public string? Hotel { get; set; }
        public double Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }
        public string? DepartureCity { get; set; }
        public bool FullBoard { get; set; }
    }
}
