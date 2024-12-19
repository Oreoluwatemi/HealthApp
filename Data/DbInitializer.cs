using System;
using System.Linq;
using OreoHealth.Models;

namespace OreoHealth.Data
{
    public class DbInitializer
    {
        //Initialize the database with some data
        public static void Initialize(ClinicContext context)
        {
            context.Database.EnsureCreated();

            if(context.Patients.Any())
            {
                return;
            }

            var patients = new Patient[]
            {
                new Patient{FirstName="Cynthia", LastName="Eden", PhoneNumber= "4326734578", Address="1234 Etobicoke Street Ontario", Gender="Female", Email="cynthia@gmail.com", Password="cynthia1234", BirthDate=DateTime.Parse("2003-09-01")},
                new Patient{FirstName="Dami", LastName="Peter", PhoneNumber= "4658364834", Address="1234 Crescent Ottawa", Gender="Male", Email="dami@yahoo.com", Password="dami", BirthDate=DateTime.Parse("2000-09-01")}

            };
            foreach(Patient p in patients)
            {
                context.Patients.Add(p);
            }

            context.SaveChanges();

            var doctors = new Doctor[]
            {
                new Doctor{FirstName="Maryann", LastName="Sailiu",PhoneNumber= "4326734578", Address="1234 Rogers Rd Ontario", Gender="Female", Available=true, Specialization="Couple Therapy",Email="maryann@gmail.com", Password="maryann1234" },
                new Doctor{FirstName="Gani", LastName="Rhodes",PhoneNumber= "4535264634", Address="34, Glencairn Ontario", Gender="Female", Available=true, Specialization="Couple Therapy",Email="gani@gmail.com", Password="gani" }

            };
            foreach(Doctor d in doctors)
            {
                context.Doctors.Add(d);
            }

            context.SaveChanges();

            var remarks = new Remarks[]
            {
                new Remarks{FullName = "Cynthia, Eden", description = ""},
                new Remarks{FullName = "Dami, Peter", description = ""}
            };
            foreach(Remarks r in remarks)
            {
                context.Remarks.Add(r);
            }
            context.SaveChanges();

            //var appointments = new Appointment[]
            //{
            //    new Appointment{StartDateTime=DateTime.Parse("2022-02-18"), EndDateTime=DateTime.Parse("2022-03-18"), PatientName = "Cynthia", DoctorName = "Maryann"}
            //};
            //foreach (Appointment a in appointments)
            //{
            //    context.Appointments.Add(a);
            //}
            //context.SaveChanges();
        }
    }
}
