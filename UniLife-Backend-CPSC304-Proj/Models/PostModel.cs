using System.ComponentModel.DataAnnotations;

namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class PostModel
    {
        [Required]
        public int Pid { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string PostBody { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int CreatorAid { get; set; }

        [Required]
        public int NumLikes { get; set; }

        [Required]
        public int NumDislikes { get; set; }
    }
}
