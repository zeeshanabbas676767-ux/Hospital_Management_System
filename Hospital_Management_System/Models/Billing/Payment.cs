using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.Billing
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string Method { get; set; } // Cash, Card, Insurance
    }
}
