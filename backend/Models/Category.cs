using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Category
    {
        public int Id { get; set;}
        public required string Name { get; set;}
        
        [JsonIgnore]
        public virtual ICollection<Appointment>? Appointments { get; set;}

    }
}