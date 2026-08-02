namespace backend.Models  
{
    public class Doctor
    {
        public int Id { get; set;}
        public required string FirstName { get; set;}
        public required string LastName { get; set;}
        public int? SpecialityId { get; set;}
        public virtual Speciality? Speciality{ get; set;}
        public int? ClinicId { get; set;}
        public virtual Clinic? Clinic{ get; set;}
        public virtual ICollection<Appointment>? Appointments { get; set; }
        
    }
}

namespace backend.Models
{
    public class DoctorDto
    {
        public int Id { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string ClinicName { get; set;}
        public string SpecialityName { get; set;}
    }
}