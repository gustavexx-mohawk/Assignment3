using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Assignment3.Models
{
    public class Error : Entity
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public int StatusCode { get; set; }

        public Error(int statusCode, string message) : base()
        {
            Message = message;
            StatusCode = statusCode;
        }

        override
        public string? ToString()
        {
            return $"{Message} (StatusCode: {StatusCode}, Id: {Id})";
        }
    }
}