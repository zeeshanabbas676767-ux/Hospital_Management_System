using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.Appointments
{
    public enum AppointmentStatus { Scheduled = 0, CheckedIn = 1, Completed = 2, Cancelled = 3 }

    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual HospitalManagement.Models.People.Patient Patient { get; set; }

        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual HospitalManagement.Models.People.Doctor Doctor { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
