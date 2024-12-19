using System;
using System.Collections.Generic;

namespace OreoHealth.Models
{
    //Patient model class
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }

        public string FullName
        {
            get { return FirstName + ", " + LastName; }
        }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
