using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DoctorController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;
        private readonly IConfiguration _Configuration;

        public DoctorController(ClinicDbContext ClinicDbContext, IConfiguration Configuration)
        {
            _ClinicDbContext = ClinicDbContext;
            _Configuration = Configuration;
        }

        /// <summary>
        /// Get all doctors
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            if(_ClinicDbContext.Doctors == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Doctors.ToListAsync();
        }

        /// <summary>
        /// Get one specific doctor
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Doctor>> GetDoctor(int Id)
        {
            if(_ClinicDbContext.Doctors == null)
            {
                return NotFound();
            }
            var doctors = await _ClinicDbContext.Doctors.FindAsync(Id);
            if(doctors == null)
            {
                return NotFound();
            }
            return doctors;
        }

       
        /// <summary>
        /// Search for a specific doctor through query parameter
        /// </summary>
        [HttpGet("Search")]

        public async Task<ActionResult<IEnumerable<DoctorDto>>> SearchDoctor([FromQuery] string query)
        {
           if(string.IsNullOrWhiteSpace(query))
           {
                return BadRequest("Search query is empty");
           }

           string databaseName = _Configuration["DatabaseSettings:DatabaseName"] ?? "";

           if(string.IsNullOrWhiteSpace(databaseName))
           {
                return BadRequest("Database name is not configured");
           }

           string sql = $"SELECT doctor.*, clinic.Name AS ClinicName, speciality.Name AS SpecialityName " +
               $"FROM {databaseName}.doctors AS doctor " +
               $"JOIN {databaseName}.clinics AS clinic ON doctor.ClinicId = clinic.Id " +
               $"JOIN {databaseName}.specialties AS speciality ON doctor.SpecialityId = speciality.Id " +
               $"WHERE doctor.FirstName LIKE {{0}} OR doctor.LastName LIKE {{0}}";
           
           var doctors = await _ClinicDbContext.Doctors
            .FromSqlRaw(sql, $"%{query}%")
            .Select(d => new DoctorDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                ClinicName = d.Clinic != null ? d.Clinic.Name : "Unknown",
                SpecialityName = d.Speciality != null ? d.Speciality.Name : "Unknown"
                })
                .ToListAsync();
           
           if(doctors.Count == 0)
           {
                return NotFound("No matching doctors found");
           }
           
           return doctors;
        }

        /// <summary>
        /// Creates a new doctor
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "FirstName": "John",
        ///         "LastName": "Doe",
        ///         "SpecialityId": 1,
        ///         "ClinicId": 1
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
        {
            _ClinicDbContext.Doctors.Add(doctor);
            await _ClinicDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

         /// <summary>
        /// Updates a specific doctor
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "FirstName": "Jane",
        ///         "LastName": "Doe",
        ///         "SpecialityId": 1,
        ///         "ClinicId": 1
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Doctor>> UpdateDoctor(int Id, Doctor doctor)
        {
            if(Id != doctor.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(doctor);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!DoctorExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool DoctorExist(int Id)
        {
            return (_ClinicDbContext.Doctors?.Any(d => d.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific doctor
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Doctor>> DeleteDoctor(int Id)
        {
            if(_ClinicDbContext.Doctors == null)
            {
                return NotFound();
            }
            var doctor = await _ClinicDbContext.Doctors.FindAsync(Id);
            if(doctor is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Doctors.Remove(doctor);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}