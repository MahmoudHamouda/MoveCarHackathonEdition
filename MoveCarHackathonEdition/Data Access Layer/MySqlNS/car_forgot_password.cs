using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoveCarHackathonEdition.MySqlNS
{
    [Table("movecar.car_forgot_password")]
    public class car_forgot_password
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string EMAIL { get; set; }
    }
}