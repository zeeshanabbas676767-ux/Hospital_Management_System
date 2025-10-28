using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.People
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Specialization { get; set; }

        // optional link to Department
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual HospitalManagement.Models.Hospital.Department Department { get; set; }

        public virtual ICollection<HospitalManagement.Models.Appointments.Appointment> Appointments { get; set; } = new HashSet<HospitalManagement.Models.Appointments.Appointment>();
    }
}
