namespace backend.Models
{
    public class Gender
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<Patient>? Patients { get; set; }
    }
}