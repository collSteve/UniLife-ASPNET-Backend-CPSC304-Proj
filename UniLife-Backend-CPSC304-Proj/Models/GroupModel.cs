using System.ComponentModel.DataAnnotations;

namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class GroupModel
    {
        [Required]
        public int Gid { get; set; }

  
        public string GroupName { get; set; }
        
        public string Role { get; set; }
        public int MemberCount { get; set; }
        public int Pid { get; set; }
    }

    public class GroupMemberNumObj { 
        public int Gid { get; set; }
    }

    public class GroupUpdateObj { 
        public string name { get; set; }
        public int Gid { get; set; }
    }

    public class GroupNewObj { 
        public string GroupName { get; set; }
        public int Aid { get; set; }
    
    }

    public class MemberOfObj { 
        public int Gid { get; set; }
        public int Aid { get; set; }
        public string Role { get; set; }
    }

    public class NewGroupPost { 
        public int Gid { get; set; }
        public int Pid { get; set; }

    }
   

}
