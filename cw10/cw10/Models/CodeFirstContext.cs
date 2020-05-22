using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw10.Models
{
    public class CodeFirstContext : DbContext
    {
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Medicament> Medicament { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }
       
        public CodeFirstContext(DbContextOptions<CodeFirstContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //FluentAPI
            var dictDoctor = new List<Doctor>();
            dictDoctor.Add(new Doctor { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan@o2.pl" });
            dictDoctor.Add(new Doctor { IdDoctor = 2, FirstName = "Adam", LastName = "Nowacki", Email = "adam@o2.pl" });

            modelBuilder.Entity<Doctor>(x =>
            {
                x.HasKey(y => y.IdDoctor).HasName("Doctor_PK");
                x.Property(y => y.FirstName).HasMaxLength(100).IsRequired();
                x.Property(y => y.LastName).HasMaxLength(100).IsRequired();
                x.Property(y => y.Email).HasMaxLength(100).IsRequired();
                x.HasData(dictDoctor);
            });


            //================================================================================================================================================
            var dictMedicament = new List<Medicament>();
            dictMedicament.Add(new Medicament { IdMedicament = 1, Name = "Apap", Description = "Przeciw bólowy", Type = "Tabletki" });
            dictMedicament.Add(new Medicament { IdMedicament = 2, Name = "Paracetamol", Description = "Angila - na wszystko", Type = "Tabletki" });


            modelBuilder.Entity<Medicament>(x =>
            {
                x.HasKey(y => y.IdMedicament).HasName("Medicament_PK");
                x.Property(y => y.Name).HasMaxLength(100).IsRequired();
                x.Property(y => y.Description).HasMaxLength(100).IsRequired();
                x.Property(y => y.Type).HasMaxLength(100).IsRequired();
                x.HasData(dictMedicament);

            });
            //================================================================================================================================================
            var dictPatient = new List<Patient>();
            dictPatient.Add(new Patient { IdPatient = 1, FirstName = "Maja", LastName = "Majeranek", Birthdate = new DateTime(1980, 1, 2) });
            dictPatient.Add(new Patient { IdPatient = 2, FirstName = "Kasia", LastName = "Kaszka", Birthdate = new DateTime(1990, 3, 4) });

            modelBuilder.Entity<Patient>(x =>
            {
                x.HasKey(y => y.IdPatient).HasName("Patient_PK");
                x.Property(y => y.FirstName).HasMaxLength(100).IsRequired();
                x.Property(y => y.LastName).HasMaxLength(100).IsRequired();
                x.Property(y => y.Birthdate).IsRequired();
                x.HasData(dictPatient);
            });
            //================================================================================================================================================
            var dictPrescription = new List<Prescription>();
            dictPrescription.Add(new Prescription { IdPrescription = 1, Date = new DateTime(2020, 1, 1), DueDate = new DateTime(2020, 2, 2), IdPatient = 1, IdDoctor = 2 });
            dictPrescription.Add(new Prescription { IdPrescription = 2, Date = new DateTime(2020, 3, 3), DueDate = new DateTime(2020, 4, 4), IdPatient = 2, IdDoctor = 1 });
            
            modelBuilder.Entity<Prescription>(x =>
            {
                x.HasKey(y => y.IdPrescription).HasName("Prescription_PK");
                x.Property(y => y.Date).IsRequired();
                x.Property(y => y.DueDate).IsRequired();
                
                x.HasOne(y => y.Patient)
               .WithMany(y => y.Prescriptions)
               .HasForeignKey(y => y.IdPatient)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("Prescription_Patient");

                x.HasOne(y => y.Doctor)
               .WithMany(y => y.Prescriptions)
               .HasForeignKey(y => y.IdDoctor)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("Prescription_Doctor");

                x.HasData(dictPrescription);
            });
            //================================================================================================================================================
            var dictPrescriptionMedicament = new List<PrescriptionMedicament>();
            dictPrescriptionMedicament.Add(new PrescriptionMedicament { IdMedicament = 1, IdPrescription = 2, Details = "Po zuzyciu leku wywal opakowanie do worka na papier" });
            dictPrescriptionMedicament.Add(new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 1, Dose = 10, Details = "Nie wypij wszystkiego pierwszego dnia" });

            modelBuilder.Entity<PrescriptionMedicament>(x =>
            {
                x.HasKey(y => new { y.IdMedicament, y.IdPrescription }).HasName("Prescription_Medicament_PK");             
                x.Property(y => y.Details).HasMaxLength(100).IsRequired();
                
                x.HasOne(y => y.Medicament)
               .WithMany(y => y.PrescriptionMedicament)
               .HasForeignKey(y => y.IdMedicament)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("Prescription_Medicament_Medicament");

                x.HasOne(y => y.Prescription)
               .WithMany(y => y.PrescriptionMedicament)
               .HasForeignKey(y => y.IdPrescription)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("Prescription_Medicament_Prescription");

                x.HasData(dictPrescriptionMedicament);

            });

        }
    }
}
