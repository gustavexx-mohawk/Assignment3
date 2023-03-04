using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Provider : Person
    {
        [Required]
        public int LicenseNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
