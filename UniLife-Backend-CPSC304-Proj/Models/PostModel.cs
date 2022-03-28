using System.ComponentModel.DataAnnotations;

namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class PostModel
    {
        public enum OrderByValue
        {
            CreatedDate,
            Title
        }

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

        public string? Email { get; set; }
        public string? PhoneNum { get; set; }
        public string? Address { get; set; }
    }

    public class CreateNewPostPostRequestObject
    {
        public string PostType { get; set; }
        public string postTitle { get; set; }
        public string postBody { get; set; }
        public DateTime createDate { get; set; }
        public int creatorUID { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
    }

    public class PostByCategoriesRequestObject
    {
        public string PostType { get; set; }
        public string[] Categories { get; set; }
        public PostModel.OrderByValue? OrderBy { get; set; }
        public bool? Asc { get; set; }
    }

    public class UpdatePostPutRequestObject
    {
        public int pid { get; set; }
        public string? postTitle { get; set; }
        public string? postBody { get; set; }
        public int? numLikes { get; set; }
        public int? numDislikes { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
    }

    public class CreateCommentPostRequestObject
    {
        public int pid { get; set; }
        public int creatorUid { get; set; }
        public string commentBody { get; set; }
    }
}
