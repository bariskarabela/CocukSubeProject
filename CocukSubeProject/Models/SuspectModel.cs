﻿using System.ComponentModel.DataAnnotations;

namespace CocukSubeProject.Models
{
    public class SuspectModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Tc { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]

        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(10)]
        public string Gender { get; set; }
        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }
        [Required]

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]

        public string District { get; set; }
        [Required]

        [StringLength(250)]
        public string CatchAdress { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}