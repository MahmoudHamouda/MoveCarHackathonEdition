using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoveCarHackathonEdition.MySqlNS
{
    [Table("movecar.car_notification")]
    public class car_notification
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string MOBILE_ID { get; set; }

        public int? REQUEST_ID { get; set; }

        [Required]
        [StringLength(300)]
        public string NOTIFICATION_MSG { get; set; }

        public virtual car_request car_request { get; set; }
    }
}