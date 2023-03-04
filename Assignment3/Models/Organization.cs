using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Organization:Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Type
        {
            get { return Type; }

            set
            {
                if (value == "Hospital" || value == "Clinic" || value == "Pharmacy")
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
        public string Address { get; set; }

        public Organization(string name, string type, string address) : base()
        {
            Name = name;
            Type = type;
            Address = address;
        }
    }
}
