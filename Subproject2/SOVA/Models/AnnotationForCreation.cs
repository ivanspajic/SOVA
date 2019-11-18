using System.ComponentModel.DataAnnotations;

namespace SOVA.Models
{
    public class AnnotationForCreation
    {
        [Required]
        public string AnnotationString { get; set; }
    }
}
