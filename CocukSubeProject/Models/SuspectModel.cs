﻿using System.ComponentModel.DataAnnotations;

namespace CocukSubeProject.Models
{
    public class SuspectModel

    {
        [Key]
        [Required(ErrorMessage ="Zorunlu Alan.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [MaxLength(11, ErrorMessage = "11 Hane olmalıdır.")]
        [MinLength(11, ErrorMessage = "11 Hane olmalıdır.")]
        [StringLength(11, ErrorMessage = "11 Hane olmalıdır.")]
        public string Tc { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [StringLength(50)]
        [MinLength(3, ErrorMessage = "Minumum 3 Hane olmalıdır.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [StringLength(50)]
        [MinLength(2, ErrorMessage = "Minumum 2 Hane olmalıdır.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]

        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [StringLength(10)]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [StringLength(50)]
        public string Nationality { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [StringLength(50)]
        public string District { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]

        [StringLength(100)]
        public string CatchAdress { get; set; }
        [Required(ErrorMessage ="Zorunlu Alan.")]
        public DateTime CatchDate { get; set; }


        public string? Done { get; set; }
        [StringLength(30, ErrorMessage = "11 Hane olmalıdır.")]
        [MaxLength(11, ErrorMessage = "11 Hane olmalıdır.")]
        [MinLength(11, ErrorMessage = "11 Hane olmalıdır.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan.")]
        [StringLength(100)]
        public string Crime { get; set; }
    }
    public class MukerrerModel
    { 
    
        public string Tc { get; set; }
       
        public string Name { get; set; }
    
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Ages { get; set; }

        public string Gender { get; set; }
        public string Nationality { get; set; }
        public int Counting { get; set; }


    }
}
