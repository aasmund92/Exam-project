using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly ClinicDbContext _ClinicDbContext;

        public CategoryController(ClinicDbContext ClinicDbContext)
        {
            _ClinicDbContext = ClinicDbContext;
        }
        
        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            if(_ClinicDbContext.Categories == null)
            {
                return NotFound();
            }
            return await _ClinicDbContext.Categories.ToListAsync();
        }

        /// <summary>
        /// Get one specific category
        /// </summary>
        [HttpGet("{Id}")]

        public async Task<ActionResult<Category>> GetCategory(int Id)
        {
            if(_ClinicDbContext.Categories == null)
            {
                return NotFound();
            }
            var category = await _ClinicDbContext.Categories.FindAsync(Id);
            if(category == null)
            {
                return NotFound();
            }
            return category;
        }
        
        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "Consulting"
        ///         
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPost]

        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            _ClinicDbContext.Categories.Add(category);
            await _ClinicDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        /// <summary>
        /// Updates a specific category
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         
        ///         "Name": "Vaccination"
        ///         
        ///         
        ///     }   
        ///     
        /// </remarks>
        [HttpPut("{Id:int}")]

        public async Task<ActionResult<Category>> UpdateCategory(int Id, Category category)
        {
            if(Id != category.Id)
            {
                return BadRequest();
            }

            _ClinicDbContext.Update(category);
            try
            {
                await _ClinicDbContext.SaveChangesAsync();
            }
            catch
            {
                if(!CategoryExist(Id))
                { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool CategoryExist(int Id)
        {
            return (_ClinicDbContext.Categories?.Any(c => c.Id == Id)).GetValueOrDefault();
        }

        /// <summary>
        /// Deletes a specific category
        /// </summary>
        [HttpDelete("{Id}")]

        public async Task<ActionResult<Category>> DeleteCategory(int Id)
        {
            if(_ClinicDbContext.Categories == null)
            {
                return NotFound();
            }
            var category = await _ClinicDbContext.Categories.FindAsync(Id);
            if(category is null)
            {
                return NotFound();
            }
            _ClinicDbContext.Categories.Remove(category);
            await _ClinicDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}