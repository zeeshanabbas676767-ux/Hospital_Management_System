namespace Hospital_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Notes = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Specialization = c.String(maxLength: 200),
                        DepartmentId = c.Int(),
                    })
                .PrimaryKey(t => t.DoctorId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 500),
                        HospitalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .ForeignKey("dbo.Hospitals", t => t.HospitalId)
                .Index(t => t.HospitalId);
            
            CreateTable(
                "dbo.Hospitals",
                c => new
                    {
                        HospitalId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Address = c.String(maxLength: 500),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.HospitalId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        DateOfBirth = c.DateTime(),
                        Email = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 50),
                        PatientNumber = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.PatientId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStatus = c.Int(nullable: false),
                        AppointmentId = c.Int(),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.AppointmentId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidAt = c.DateTime(nullable: false),
                        Method = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.InvoiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Invoices", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Departments", "HospitalId", "dbo.Hospitals");
            DropForeignKey("dbo.Doctors", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Payments", new[] { "InvoiceId" });
            DropIndex("dbo.Invoices", new[] { "PatientId" });
            DropIndex("dbo.Invoices", new[] { "AppointmentId" });
            DropIndex("dbo.Departments", new[] { "HospitalId" });
            DropIndex("dbo.Doctors", new[] { "DepartmentId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropTable("dbo.Payments");
            DropTable("dbo.Invoices");
            DropTable("dbo.Patients");
            DropTable("dbo.Hospitals");
            DropTable("dbo.Departments");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
