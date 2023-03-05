using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Patient:Person
    {
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }        
        
        public Patient(int year, int month, int day)
        {
            DateOfBirth = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        }
    }
}
