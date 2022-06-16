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

        public int CountryId { get; set; }

        [Required]
        [ForeignKey("CountryId")]
        public virtual Country? Country{ get; set; }

        public City(){}

        public City(string? name, Country? country)
        {
            Name = name;
            Country = country;
        }
    }
}
