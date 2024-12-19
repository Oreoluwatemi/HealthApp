using System;
using System.ComponentModel.DataAnnotations;

namespace OreoHealth.Models
{
    public class AdminSession
    {
        [Key]
        public string SessionId { get; set; }
        public int DoctorId { get; set; }
        public DateTime LoginDate { get; set; }
        public Doctor Doctor { get; set; }
    }
}
