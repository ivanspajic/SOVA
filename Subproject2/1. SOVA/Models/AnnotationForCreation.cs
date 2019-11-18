using System.ComponentModel.DataAnnotations;

namespace _1._SOVA.Models
{
    public class AnnotationForCreation
    {
        [Required]
        public string AnnotationString { get; set; }
    }
}
