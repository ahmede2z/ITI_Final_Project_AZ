using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ErenYeager.Models {
    public class Teacher {
        public int Id {
            get; set;
        }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "u have 2 provide a valid name")]
        [MinLength(7, ErrorMessage = "full name mustn't be less than 7 charachters")]
        [MaxLength(50, ErrorMessage = "full name mustn't exceed 50 charachters")]
        public string? FullName {
            get; set;
        }
        [Required(ErrorMessage = "u have 2 provide a valid Degree")]
        [MinLength(2, ErrorMessage = "Degree mustn't be less than 2 charachters")]
        [MaxLength(20, ErrorMessage = "Degree mustn't exceed 20 charachters")]
        public string? Degree {
            get; set;
        }
        [Required(ErrorMessage = "u have 2 provide a valid salary.")]
        [Range(2500, 25000, ErrorMessage = "salary must be between 2500 and 25000")]
        public double? Salary {
            get; set;
        }
        [Display(Name = "Department")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a valid department")]
        public int DepartmentID { get; set; }

        [ValidateNever]
        public Department Department { get; set; }

        [Display(Name = "Image")]
        [ValidateNever]
        public string ImageUrl { get; set; }

    }

}