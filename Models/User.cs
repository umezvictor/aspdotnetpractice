using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


// this file is not in use presently//
namespace FileManagerApp.Models
{
    // this view model -- carries data from the user registration view to the controller
    // it's optional to have it. 
    public class User
    {

        [Key]// serves as the primary key
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password",
            ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
