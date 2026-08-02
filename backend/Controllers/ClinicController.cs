using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClinicController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;

        public ClinicController(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }

        /// <summary>
        /// Get all clinics
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Clinic>>> GetClinics()
        {
            if(_ClinicDbContext.Clinics == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Clinics.ToListAsync();
        }

        /// <summary>
        /// Get one specific clinic
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Clinic>> GetClinic(int Id)
        {
            if(_ClinicDbContext.Clinics == null)
            {
                return NotFound();
            }
            var clinic = await _ClinicDbContext.Clinics.FindAsync(Id);
            if(clinic == null)
            {
                return NotFound();
            }
            return clinic;
        }

        /// <summary>
        /// Creates a new clinic
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "St.Olavs Hospital",
        ///         "Address": "St.Olavs gt.123",
        ///         "PhoneNumber": "123456789",
        ///         "Email": "St.Olavs.Hospital@hotmail.com"
        ///        
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Clinic>> CreateClinic(Clinic clinic)
        {
            _ClinicDbContext.Clinics.Add(clinic);
            await _ClinicDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClinic), new { id = clinic.Id }, clinic);
        }   

        /// <summary>
        /// Updates a specific clinic
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "St.Olavs Hospital private clinic",
        ///         "Address": "St.Olavs gt.125",
        ///         "PhoneNumber": "123456789",
        ///         "Email": "St.Olavs.Hospital@hotmail.com"
        ///        
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Clinic>> UpdateClinic(int Id, Clinic clinic)
        {
            if(Id != clinic.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(clinic);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!ClinicExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool ClinicExist(int Id)
        {
            return (_ClinicDbContext.Clinics?.Any(c => c.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific clinic
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Clinic>> DeleteClinic(int Id)
        {
            if(_ClinicDbContext.Clinics == null)
            {
                return NotFound();
            }
            var clinic = await _ClinicDbContext.Clinics.FindAsync(Id);
            if(clinic is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Clinics.Remove(clinic);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}