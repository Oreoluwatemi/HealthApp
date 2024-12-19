using System;
namespace OreoHealth.Models
{
    //Appointment model class
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string PatientName { get; set; }
        public Patient Patient { get; set; }
        public string DoctorName{ get; set; }
        public Doctor Doctor { get; set; }

    }
}
