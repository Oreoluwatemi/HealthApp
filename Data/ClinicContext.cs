using System;
using Microsoft.EntityFrameworkCore;
using OreoHealth.Models;

namespace OreoHealth.Data
{
    //A database context is created
    public class ClinicContext : DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<AdminSession> AdminSessions { get; set; }
        public DbSet<Remarks> Remarks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Session>().ToTable("Session");
            modelBuilder.Entity<AdminSession>().ToTable("AdminSession");
            modelBuilder.Entity<Remarks>().ToTable("Remarks");
        }
    }
}
