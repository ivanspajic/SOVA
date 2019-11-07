using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1._SOVA.Models
{
    public class AnnotationForCreation
    {
        [Required]
        public string AnnotationString { get; set; }
    }
}
