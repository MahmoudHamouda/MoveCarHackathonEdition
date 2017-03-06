using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MoveCarHackathonEdition.MySqlNS
{
    [Table("movecar.car_request")]
    public class car_request
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public car_request()
        {
            car_notification = new HashSet<car_notification>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string PLATE_NUMBER { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime REQUEST_DATE { get; set; }

        [Column(TypeName = "enum")]
        [Required]
        [StringLength(65532)]
        public string STATUS { get; set; }

        [Required]
        [StringLength(50)]
        public string CAR_USERNAME { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<car_notification> car_notification { get; set; }
    }
}