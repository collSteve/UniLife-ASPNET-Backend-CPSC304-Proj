using System.ComponentModel.DataAnnotations;

namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class GroupModel
    {
        [Required]
        public int Gid { get; set; }

        [Required]
        public string Group_Name { get; set; }

        
    }
}
