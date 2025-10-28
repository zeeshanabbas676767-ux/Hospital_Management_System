using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.Billing
{
    public enum PaymentStatus { Pending = 0, Paid = 1, PartiallyPaid = 2, Refunded = 3 }

    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal Amount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // optional link to appointment
        public int? AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public virtual HospitalManagement.Models.Appointments.Appointment Appointment { get; set; }

        // invoice belongs to a patient
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual HospitalManagement.Models.People.Patient Patient { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
