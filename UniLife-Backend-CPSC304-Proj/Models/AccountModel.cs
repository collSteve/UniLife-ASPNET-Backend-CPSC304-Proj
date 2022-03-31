using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class AccountModel
    {
        [Required]
        public int Aid { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }

    public class CreateNewAccountRequestObj
    {
        public string AccountType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class accGroupObj
    {
        public int AID { get; set; }
        public int GID { get; set; }
        public string Role { get; set; }
    }
    public class UpdateAccountRequestObj
    {
        public int Aid { get; set; }
        public string username { get; set; }
        public string password { get; set; } 
        public string email { get; set; }
        public float seller_rating { get; set; }
    }

}
