using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class School
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!; // EF will populate / null-forgiving

        [Required]
        [StringLength(100)]
        public string Principal { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string Address { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Student> Students { get; set; } = new();
    }
}