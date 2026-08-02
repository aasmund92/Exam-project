namespace backend.Models
{
    public class Speciality 
    {
        public int Id { get; set;}
        public required string Name { get; set;}
        public virtual ICollection<Doctor>? Doctors { get; set;}
    }
}