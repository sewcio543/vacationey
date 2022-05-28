using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HotelId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public City? City { get; set; }

        public bool Pool { get; set; }
        public bool WiFi { get; set; }


        List<Offer> Offers = new List<Offer>();

        public Hotel(){}

        public Hotel(int hotelId, string? name, string? description, City? city, bool pool, bool wiFi)
        {
            HotelId = hotelId;
            Name = name;
            Description = description;
            City = city;
            Pool = pool;
            WiFi = wiFi;
        }

    }
}
