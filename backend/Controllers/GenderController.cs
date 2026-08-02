using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GenderController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;

        public GenderController(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }

        /// <summary>
        /// Get all genders
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Gender>>> GetGenders()
        {
            if(_ClinicDbContext.Genders == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Genders.ToListAsync();
        }

        /// <summary>
        /// Get one specific gender
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Gender>> GetGender(int Id)
        {
            if(_ClinicDbContext.Genders == null)
            {
                return NotFound();
            }
            var gender = await _ClinicDbContext.Genders.FindAsync(Id);
            if(gender == null)
            {
                return NotFound();
            }
            return gender;
        }

        /// <summary>
        /// Creates a new gender
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "Male"
        ///    
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Gender>> CreateGender(Gender gender)
        {
            _ClinicDbContext.Genders.Add(gender);
            await _ClinicDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGender), new { id = gender.Id }, gender);
        }

        /// <summary>
        /// Updates a specific gender
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "Female"
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Gender>> UpdateGender(int Id, Gender gender)
        {
            if(Id != gender.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(gender);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!GenderExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool GenderExist(int Id)
        {
            return (_ClinicDbContext.Genders?.Any(g => g.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific gender
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Gender>> DeleteGender(int Id)
        {
            if(_ClinicDbContext.Genders == null)
            {
                return NotFound();
            }
            var gender = await _ClinicDbContext.Genders.FindAsync(Id);
            if(gender is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Genders.Remove(gender);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}