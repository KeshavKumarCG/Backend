// using System.ComponentModel.DataAnnotations;

// namespace Backend.Models
// {
//     public class CarPatchRequest
//     {
//         public string CarID { get; set; }
//         public string StatusID { get; set; }

//         public string CarModel { get; set; }
//     }
// }


using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CarPatchRequest
    {
        [MaxLength(50)]
        public string CarID { get; set; } // Optional if CarNumber is provided

        [Required]
        [MaxLength(50)]
        public string StatusID { get; set; } // Required

        [MaxLength(50)]
        public string CarNumber { get; set; } // Optional if CarID is provided
    }
}
