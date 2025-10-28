using System.Data.Entity;
using HospitalManagement.Models.Hospital;
using HospitalManagement.Models.People;
using HospitalManagement.Models.Appointments;


namespace HospitalManagement.Models
{
    public class HospitalContext : DbContext
    {
        public HospitalContext() : base("HospitalManagement") { }

        public DbSet<HospitalManagement.Models.Hospital.Hospital> Hospitals { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Billing.Invoice> Invoices { get; set; }
        public DbSet<Billing.Payment> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent cascade delete for safety on common relationships
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
        }
    }
}
