using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerApp.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        // this id is of string type. it looks like monogodb id
        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }

        // this property is a collection property, so it has to be initialized above in a contructor
        // else it will throw an exception
        // Users property is used to display users that belong to 
        // a particular role. it will hold the list of usernames
        public List<string> Users { get; set; }

    }
}
