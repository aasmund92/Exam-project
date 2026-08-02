using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Clinic 
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        
        [EmailAddress]
        public required string Email { get; set; }

        public virtual ICollection<Doctor>? Doctors { get; set; }
    }
}