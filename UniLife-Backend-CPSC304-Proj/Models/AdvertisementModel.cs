using System.ComponentModel.DataAnnotations;

namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class AdvertisementModel
    {
        public enum OrderByValue
        {
            CreatedDate,
            Title
        }

        [Required]
        public int Adid { get; set; }
        
        public string ad_description { get; set; }

        public float price { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public int clicks { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }

    public class CreateNewAdRequestObj
    {
        public string ad_description { get; set; }
        public float price { get; set; }
        public string title { get; set; }
        public DateTime createDate { get; set; }
    }
}
