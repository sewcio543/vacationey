using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class Offer
    {
        public Offer(string? name, Hotel? hotel, decimal price, DateTime dateFrom, DateTime dateTo, City? 
            departureCity, City? arrivalCity, bool fullBoard, int numberOfPeople)
        {
            Hotel = hotel;
            Price = price;
            DateFrom = dateFrom;
            DateTo = dateTo;
            DepartureCity = departureCity;
            FullBoard = fullBoard;
        }
        public Offer() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfferId { get; set; }


        public int HotelId { get; set; }

        [Required]
        public Hotel? Hotel { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }

        public int DepartureCityId { get; set; }

        public City? DepartureCity { get; set; }

        public bool FullBoard { get; set; }

    }
}
