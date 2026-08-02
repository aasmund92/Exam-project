using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SpecialityController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;

        public SpecialityController(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }

        /// <summary>
        /// Get all specialties
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Speciality>>> GetSpecialties()
        {
            if(_ClinicDbContext.Specialties == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Specialties.ToListAsync();
        }

        /// <summary>
        /// Get one specific speciality
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Speciality>> GetSpeciality(int Id)
        {
            if(_ClinicDbContext.Specialties == null)
            {
                return NotFound();
            }
            var speciality = await _ClinicDbContext.Specialties.FindAsync(Id);
            if(speciality == null)
            {
                return NotFound();
            }
            return speciality;
        }

        /// <summary>
        /// Creates a new speciality
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "Surgery"
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Speciality>> CreateSpeciality(Speciality speciality)
        {
            _ClinicDbContext.Specialties.Add(speciality);
            await _ClinicDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSpeciality), new { id = speciality.Id }, speciality);
        }

        /// <summary>
        /// Updates a specific speciality
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///      
        ///         "Name": "Cardiology"
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Speciality>> UpdateSpeciality(int Id, Speciality speciality)
        {
            if(Id != speciality.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(speciality);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!SpecialityExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool SpecialityExist(int Id)
        {
            return (_ClinicDbContext.Specialties?.Any(s => s.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific speciality
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Speciality>> DeleteSpeciality(int Id)
        {
            if(_ClinicDbContext.Specialties == null)
            {
                return NotFound();
            }
            var speciality = await _ClinicDbContext.Specialties.FindAsync(Id);
            if(speciality is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Specialties.Remove(speciality);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}