﻿using System;
using System.ComponentModel.DataAnnotations;


namespace cw10.DTOs
{
    public class ModifyStudentRequest
    {
        [Required]
        public string IndexNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public int IdEnrollment { get; set; }
    }


}