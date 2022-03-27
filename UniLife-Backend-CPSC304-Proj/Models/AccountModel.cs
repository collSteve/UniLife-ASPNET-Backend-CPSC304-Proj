
using System.ComponentModel.DataAnnotations;
namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class AccountModel
    {
         [Required]   
         public int AID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class accGroupObj { 
        public int AID { get; set; }
        public int GID { get; set; }
    }
}
