using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReligionController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;

        public ReligionController(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }

        /// <summary>
        /// Get all religions
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Religion>>> GetReligions()
        {
            if(_ClinicDbContext.Religions == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Religions.ToListAsync();
        }

        /// <summary>
        /// Get one specific religion
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Religion>> GetReligion(int Id)
        {
            if(_ClinicDbContext.Religions == null)
            {
                return NotFound();
            }
            var religion = await _ClinicDbContext.Religions.FindAsync(Id);
            if(religion == null)
            {
                return NotFound();
            }
            return religion;
        }

        /// <summary>
        /// Creates a new religion
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///        
        ///         "Name": "Christian"
        ///    
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Religion>> CreateReligion(Religion religion)
        {
            _ClinicDbContext.Religions.Add(religion);
            await _ClinicDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReligion), new { id = religion.Id }, religion);
        }

        /// <summary>
        /// Updates a specific religion
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "Muslim"
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Religion>> UpdateReligion(int Id, Religion religion)
        {
            if(Id != religion.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(religion);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!ReligionExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool ReligionExist(int Id)
        {
            return (_ClinicDbContext.Religions?.Any(r => r.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific religion
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Religion>> DeleteReligion(int Id)
        {
            if(_ClinicDbContext.Religions == null)
            {
                return NotFound();
            }
            var religion = await _ClinicDbContext.Religions.FindAsync(Id);
            if(religion is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Religions.Remove(religion);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}