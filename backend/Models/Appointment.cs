using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public required DateTime AppointmentTime { get; set; }
        public required TimeSpan Duration { get; set; }
        public int? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        public int? CategoryId { get; set; }
        
        [JsonIgnore]
        public virtual Category? Category{ get; set; }
        public int? DoctorId { get; set; }
        public virtual Doctor? Doctor{ get; set; }
    }
}