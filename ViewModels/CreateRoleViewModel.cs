using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerApp.ViewModels
{
    public class CreateRoleViewModel
    {
        // this class is the model for the role table
        [Required]
        public string RoleName { get; set; }
    }
}
