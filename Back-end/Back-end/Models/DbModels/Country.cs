using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public Country(){ }

        public Country(int countryId, string? name)
        {
            CountryId = countryId;
            Name = name;
        }
    }
}
