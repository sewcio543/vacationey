using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        [DataType(DataType.Password)]
        // TODO checks
        public string? Password { get; set; }

        [Required]
        [StringLength(50)]
        public string? Login { get; set; }

        public Admin() { }

        public Admin(int adminId, string? password, string? login)
        {
            AdminId = adminId;
            Password = password;
            Login = login;
        }
    }
}
