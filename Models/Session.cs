using System;
namespace OreoHealth.Models
{
    public class Session
    {
        public string SessionId { get; set; }
        public int PatientId { get; set; }
        public DateTime LoginDate { get; set; }
        public Patient Patient { get; set; }
    }
}
