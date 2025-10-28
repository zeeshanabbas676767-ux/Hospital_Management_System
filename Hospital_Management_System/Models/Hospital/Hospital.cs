using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.Hospital
{
    public class Hospital
    {
        [Key]
        public int HospitalId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();
    }
}
