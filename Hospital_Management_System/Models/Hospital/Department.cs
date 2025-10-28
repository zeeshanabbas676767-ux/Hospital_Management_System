using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.Hospital
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Foreign key to Hospital
        public int HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

        public virtual ICollection<HospitalManagement.Models.People.Doctor> Doctors { get; set; } = new HashSet<HospitalManagement.Models.People.Doctor>();
    }
}
