using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoveCarHackathonEdition.MySqlNS
{
    [Table("movecar.car_user")]
    public class car_user
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string CAR_USERNAME { get; set; }

        [Required]
        [StringLength(25)]
        public string PASSWORD { get; set; }

        [Required]
        [StringLength(100)]
        public string MOBILE_ID { get; set; }
    }
}