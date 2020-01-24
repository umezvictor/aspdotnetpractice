using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManagerApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FileManagerApp.Controllers
{
    [Authorize(Roles = "Admin")] // only admin can access this controller
    public class AdministrationController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, 
                    UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        } 

        //create role
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                //create a new role using the idenetityrole class
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                // redirect user if role is created successfully
                if (result.Succeeded)
                {
                    // redirect to roles page -- page that displays all roles
                    return RedirectToAction("ListRoles", "Administration");
                }

                // if errors
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        //list all roles
        //Roles property of the Rolesmanager class returns an Iqueryable of Identityroles object
        //which is what is passed to the view 
        // nb. in the view, set the model to Ienumerable of identityRole
        // because the interface Iqueryable implements Ienumerable

    
        [HttpGet]
        public IActionResult ListRoles()
        {
            //fetch the roles from the roles table and display in view
            var roles = roleManager.Roles;
            return View(roles);
        } 

        // returns list of roles to be edited
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // find role by incoming id
            var role = await roleManager.FindByIdAsync(id);

            // check if role is null
            if(role == null)
            {
                // if role id is not found. rdirect user to 404 page with error message
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
               

            // if role is found
            //create an instance or object of EditRoleViewModel class
            // to populate the Users propety, we need the Identityapi usermanger service
            //using dependency injection
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            //to populate the Users property, use the IdentityApi UserManager service
            //inject it into the Administrtation controller using its constructor
            //loop through the users returned by the usermanager service
            // retrieve all the users using the injected userManager service

            foreach (var user in userManager.Users)
            {
                // check if the user been iterated over belongs to the role being edited
                // this methid returns true if user belongs to the role
               if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    //if success, add it to the Users collection of the Editroleview class
                    model.Users.Add(user.UserName);// so we have Id, RoleName and Users in that object
                }
            }

            return View(model);

        }

        // update edited roles
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            // check if role still exists
            var role = await roleManager.FindByIdAsync(model.Id);

            // check if role is null
            if (role == null)
            {
                // if role id is not found. rdirect user to 404 page with error message
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                //update role
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                // if errors, 
                
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                //rerender edit view if there are validation errors
                return View(model);

            }



        }

        //view to display users assigned to each role and edit
        //them. takes in the roleId from the query string as a paramater
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            //store the roleId in the viewbag so it can be accessed when required
            ViewBag.roleId = roleId;

            //find the role
            var role = await roleManager.FindByIdAsync(roleId);
            
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            // if role is found
            //create an instance of UserRoleViewModel class
            //we need a list of userroleviewmodel objects

            var model = new List<UserRoleViewModel>();


            // first, loop through the list of all users
            //for each user, create an instance of userroleviewmodel class

            foreach(var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                // check if the user is a member of a givem role
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    //set the value for the checkbox to true
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                //whike looping throught the users and checking other stuff
                //add theuser to the list of Userroleviewmodel
                model.Add(userRoleViewModel);
            }

            // pass the model object to the view to render
            return View(model);

        }

        // method to update users in role
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            //find the user by the roleid
            var role = await roleManager.FindByIdAsync(roleId);

            //redirect user if role is null
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            // if role is found
            for(int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                //add selected user to role only if he is selected and also
                // not already a member of the role
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }

                // remove user from role if not selected and already a member of the role
                if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);

                }
                else
                {
                    //continue processing if:
                    //1 if user is selected and he's already in the role then do nothing
                    //2 if user is not selected and not in the role, do nothing as well
                    // hence we use continue keyword, which goes back and processes the next user
                    continue;
                }

                //check if AddTorole or romovefromrole completed succesfully
                if (result.Succeeded)
                {
                    //check if we have reached the end of the loop
                    if (i < (model.Count - 1))
                        continue; // we still have items to loop over
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }

            }

            //if model object is empty
            //redirect to EditRole
            return RedirectToAction("EditRole", new { Id = roleId });
        }

    } 
}
