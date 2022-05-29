using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.DbModels
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string? SurName { get; set; }


        [Required]
        [StringLength(50, MinimumLength =10)]
        [DataType(DataType.Password)]
        // TODO checks
        public string? Password { get; set; }

        [Required]
        [StringLength(50)]
        public string? Login { get; set; }

        public User(){ }

        public User(int userId, string? name, string? surName, string? password, string? login)
        {
            UserId = userId;
            Name = name;
            SurName = surName;
            Password = password;
            Login = login;
        }
    }
}
