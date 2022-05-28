using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public Region? Region { get; set; }

        public City(){}

        public City(int cityId, string? name, Region? region)
        {
            CityId = cityId;
            Name = name;
            Region = region;
        }
    }
}
