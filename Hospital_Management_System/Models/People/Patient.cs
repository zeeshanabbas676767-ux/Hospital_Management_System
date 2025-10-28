using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.People
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string PatientNumber { get; set; }

        public virtual ICollection<HospitalManagement.Models.Appointments.Appointment> Appointments { get; set; } = new HashSet<HospitalManagement.Models.Appointments.Appointment>();

        public virtual ICollection<HospitalManagement.Models.Billing.Invoice> Invoices { get; set; } = new HashSet<HospitalManagement.Models.Billing.Invoice>();
    }
}
