using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Patient
    {
        public int Id { get; set; }
        private string _firstName;
        public required string FirstName 
        {   get => _firstName; 
            set => _firstName = CapitalizeFirstLetter(value); 
        }
        private string _lastName;
        public required string LastName 
        { 
            get => _lastName; 
            set => _lastName = CapitalizeFirstLetter(value); 
        }
        private string _email;
        
        [EmailAddress]
        public required string Email 
        {
             get => _email; 
             set => _email = value.ToLower(); 
        }
        
        public DateOnly Birthday { get; set; }
        public int? GenderId { get; set; }
        public virtual Gender? Gender{ get; set; }
        public int? ReligionId { get; set; }
        public virtual Religion? Religion{ get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
         private static string CapitalizeFirstLetter(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        return char.ToUpper(value[0]) + value.Substring(1);
    }
    }
}