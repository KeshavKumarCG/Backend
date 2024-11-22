using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CarPatchRequest
    {
        public string CarID { get; set; }
        public string StatusID { get; set; }
    }
}
