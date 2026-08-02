using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PatientController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;

        public PatientController(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }

        /// <summary>
        /// Get all patients
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            if(_ClinicDbContext.Patients == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Patients.ToListAsync();
        }

        /// <summary>
        /// Get one specific patient
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Patient>> GetPatient(int Id)
        {
            if(_ClinicDbContext.Patients == null)
            {
                return NotFound();
            }
            var patient = await _ClinicDbContext.Patients.FindAsync(Id);
            if(patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        /// <summary>
        /// Creates a new patient
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///        
        ///         "FirstName": "John",
        ///         "LastName": "Doe",
        ///         "Email": "John.Doe@hotmail.com",
        ///         "Birthday": "1992-01-01",
        ///         "GenderId" : 1,
        ///         "ReligionId" : 1
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            bool emailExists = await _ClinicDbContext.Patients.AnyAsync(p => p.Email == patient.Email);
            if(emailExists)
            {
                return BadRequest("Email already exists with another patient");
            }

            _ClinicDbContext.Patients.Add(patient);
            await _ClinicDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        /// <summary>
        /// Updates a specific patient
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "FirstName": "Jane",
        ///         "LastName": "Doe",
        ///         "Email": "Jane.Doe@hotmail.com",
        ///         "Birthday": "1992-01-01",
        ///         "GenderId" : 2,
        ///         "ReligionId" : 1
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Patient>> UpdatePatient(int Id, Patient patient)
        {
            if(Id != patient.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(patient);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!PatientExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool PatientExist(int Id)
        {
            return (_ClinicDbContext.Patients?.Any(p => p.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific patient
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Patient>> DeletePatient(int Id)
        {
            if(_ClinicDbContext.Patients == null)
            {
                return NotFound();
            }
            var patient = await _ClinicDbContext.Patients.FindAsync(Id);
            if(patient is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Patients.Remove(patient);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}