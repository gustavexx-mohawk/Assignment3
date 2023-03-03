using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Organization : Entity
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public string Type {
            get { return Type; }

            set
            {
                if (value.Equals("Hospital") || value.Equals("Clinic") || value.Equals("Pharmacy"))
                {
                    Type = value;
                }
                else
                {
                    throw new Exception("Type must be Hospital, Clinic, or Pharmacy.");
                }
            }
        }

        [Required]
        public String Address { get; set; }
    }
}
