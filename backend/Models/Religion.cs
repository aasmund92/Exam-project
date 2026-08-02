namespace backend.Models
{
    public class Religion
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<Patient>? Patients { get; set; }
    }
}