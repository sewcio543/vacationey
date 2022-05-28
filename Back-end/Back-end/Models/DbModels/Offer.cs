using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class Offer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfferId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public Hotel? Hotel { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        public City? DepartureCity { get; set; }

        public City? ArrivalCity { get; set; }

        public bool FullBoard { get; set; }

        //default 1
        public int NumberOfPeople { get; set; }

        [Column("CreatedBy")]
        public Admin? Admin { get; set; }
    }
}
