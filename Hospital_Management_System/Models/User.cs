namespace HospitalManagement.Models
{ 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml;

    [Table("User")]
    public partial class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(150)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;

        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(500)]
        public string ProfileImagePath { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
