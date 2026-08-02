using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Speciality> Specialties { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Religion> Religions { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName)
                    .IsRequired();
                entity.Property(e => e.LastName)
                    .IsRequired();
                entity.Property(e => e.Email)
                    .IsRequired();
                entity.Property(e => e.Birthday)
                    .IsRequired();

                entity.HasIndex(e => e.Email)
                    .IsUnique();    
                
                
                
                entity.HasOne(g => g.Gender)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(g => g.GenderId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.Religion)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(r => r.ReligionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired();
                
                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired();
                
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired();
                
                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired();
                
                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

             modelBuilder.Entity<Clinic>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired();
                entity.Property(e => e.Address)
                    .IsRequired();
                entity.Property(e => e.PhoneNumber)
                    .IsRequired();
                entity.Property(e => e.Email)
                    .IsRequired();
                
                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName)
                    .IsRequired();
                entity.Property(e => e.LastName)
                    .IsRequired();
                
                entity.HasOne(s => s.Speciality)
                    .WithMany(d => d.Doctors)
                    .HasForeignKey(s => s.SpecialityId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(c => c.Clinic)
                    .WithMany(d => d.Doctors)
                    .HasForeignKey(c => c.ClinicId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

           modelBuilder.Entity<Appointment>(entity =>
           {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AppointmentTime)
                    .IsRequired();
                entity.Property(e => e.Duration)
                    .IsRequired();
                entity.HasOne(p => p.Patient)
                    .WithMany(a => a.Appointments)
                    .HasForeignKey(p => p.PatientId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(c => c.Category)
                    .WithMany(a => a.Appointments)
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(d => d.Doctor)
                    .WithMany(a => a.Appointments)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => e.PatientId);

                entity.HasIndex(e => new { e.PatientId, e.AppointmentTime})
                    .IsUnique();
           });
        }
    }
}