using System.ComponentModel.DataAnnotations;

namespace UniLife_Backend_CPSC304_Proj.Models
{
    public class AdvertisementModel
    {
        [Required]
        public int Adid { get; set; }
        
        public string ad_description { get; set; }

        public float price { get; set; }
        
        [Required]
        public string title { get; set; }

        [Required]
        public int clicks { get; set; }
    }

    public class CreateNewAdRequestObj
    {
        public string ad_description { get; set; }
        public float price { get; set; }
        public string title { get; set; }
    }
}
