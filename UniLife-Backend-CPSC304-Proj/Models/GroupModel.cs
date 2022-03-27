using System.ComponentModel.DataAnnotations;

namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class GroupModel
    {
        [Required]
        public int Gid { get; set; }

        [Required]
        public string GroupName { get; set; }
 

    }

    public class GroupNewObj { 
        public int Gid { get; set; }
        public string GroupName { get; set; }
    
    }

   

}
