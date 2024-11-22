using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{


    public class CarStatus
    {
        public string ID { get; set; }
        public string Status { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<CarStatusLog> CarStatusLogs { get; set; }
    }


}



