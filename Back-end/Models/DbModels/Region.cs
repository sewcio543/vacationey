using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class Region
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegionId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public Country? Country { get; set; }

        public Region(){ }

        public Region(int regionId, string? name, Country? country)
        {
            RegionId = regionId;
            Name = name;
            Country = country;
        }
    }
}
