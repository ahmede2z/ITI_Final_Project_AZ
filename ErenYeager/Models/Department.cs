using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ErenYeager.Models {
    public class Department {

        public int Id {
            get; set;
        }
        [Display(Name = "Department")]
        [Required(ErrorMessage = "u have 2 provide a valid name")]
        [MinLength(2, ErrorMessage = "name mustn't be less than 2 charachters")]
        [MaxLength(10, ErrorMessage = "name mustn't exceed 10 charachters")]
        public string? Name {
            get; set;
        }

        [Required(ErrorMessage = "u have 2 provide a valid Description.")]
        [MinLength(5, ErrorMessage = "Description mustn't be less than 5 charachters")]
        [MaxLength(20, ErrorMessage = "Description mustn't exceed 20 charachters")]
        public string? Description {
            get; set;
        }
        [ValidateNever]
        public List<Teacher> Teachers { get; set; }
    }
}
