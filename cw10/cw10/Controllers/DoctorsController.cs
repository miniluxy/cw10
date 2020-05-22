using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw10.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw10.Controllers
{
    

    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly CodeFirstContext context;
        //Dodawanie danych do bazy danych zrobiłem jak w przykłdzie z wykładu w klasie CodeFristContext

        //A taki sposób poniżej znalazłem w internecie
        //Nie wiem jak jest lepiej, więc zostawie tak jak było na wykładzie
        /*
        public DoctorsController(CodeFirstContext context)
        {        
            context.Doctor.Add(new Doctor { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan@o2.pl" });
            context.Doctor.Add(new Doctor { IdDoctor = 2, FirstName = "Adam", LastName = "Nowacki", Email = "adam@o2.pl" });
            context.Patient.Add(new Patient { IdPatient = 1, FirstName = "Maja", LastName = "Majeranek", Birthdate = new DateTime(1980, 1, 2) });
            context.Patient.Add(new Patient { IdPatient = 2, FirstName = "Kasia", LastName = "Kaszka", Birthdate = new DateTime(1990, 3, 4) });
            context.Medicament.Add(new Medicament { IdMedicament = 1, Name = "Apap", Description = "Przeciw bólowy", Type = "Tabletki" });
            context.Medicament.Add(new Medicament { IdMedicament = 2, Name = "Paracetamol", Description = "Angila - na wszystko", Type = "Tabletki" });
            context.Prescription.Add(new Prescription { IdPrescription = 1, Date = new DateTime(2020, 1, 1), DueDate = new DateTime(2020, 2, 2), IdPatient = 1, IdDoctor = 2 });
            context.Prescription.Add(new Prescription { IdPrescription = 2, Date = new DateTime(2020, 3, 3), DueDate = new DateTime(2020, 4, 4), IdPatient = 2, IdDoctor = 1 });
            context.PrescriptionMedicament.Add(new PrescriptionMedicament { IdMedicament = 1, IdPrescription = 2, Details = "2 razy dziennie" });
            context.PrescriptionMedicament.Add(new PrescriptionMedicament { IdMedicament = 2, IdPrescription = 1, Dose = 1, Details = "1 dziennie" });
            context.SaveChanges();
        }

        */
        //================================================================================================================================================
        [HttpGet("{IdDoctor}")]
        public IActionResult getDoctor(int? id)
        {
            if (id == null)         // sprawdza czy w linku podany jest w ogóle podane id
                return NotFound();

            Doctor d = context.Doctor.Find(id);

            if (d == null)          // sprawdza czy id istnieje w bazie danych
            {
                return NotFound("Not found");
            }else
            return Ok(d);
        }

        //================================================================================================================================================
        [HttpPut("update")]
        public IActionResult updateDoctor(Doctor doctor)
        {
            Doctor d = context.Doctor.Find(doctor.IdDoctor);
            if (d == null)
                return NotFound("Id doctor not found");
            context.Database.BeginTransaction();
            context.Entry(d).CurrentValues.SetValues(doctor);
            context.SaveChanges();
            context.Database.CommitTransaction();
            return Ok(doctor);
        }

        //================================================================================================================================================
        [HttpPost("add")]
        public IActionResult addDoctor(Doctor doctor)
        {
            context.Database.BeginTransaction();
            context.Doctor.Add(doctor);
            context.SaveChanges();
            context.Database.CommitTransaction();
            return Created("Add new", doctor);
        }

        //================================================================================================================================================
        [HttpDelete("delete/{IdDoctor}")]
        public IActionResult deleteDoctor(int? id)
        {
            if (id == null)
                return NotFound();

            Doctor d = context.Doctor.Find(id);

            if (d == null)
                return NotFound("Id doctor not found");
            context.Database.BeginTransaction();
            context.Doctor.Remove(d);
            context.SaveChanges();
            context.Database.CommitTransaction();
            return Ok();
        }

    }
}