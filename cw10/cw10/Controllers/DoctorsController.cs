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