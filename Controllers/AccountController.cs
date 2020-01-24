using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManagerApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileManagerApp.Controllers
{
    // require authorization to access all actions within this controller
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        // SignInManager class will be used to check if a user is logged in 
        // in the application navigation menu in the frontend
        // an instance of it will be created in the layout view
        // constructor -- 
        // inject usermanager of identityuser and bring in the namespace
        // do same for signinmamger
        // ctor tab tab
        // the parameter will be userManager and signInManager respectively
        // next -- generate private fields for user and signinmanager

        

        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //Logout user
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            // redirect user to home page when he logs out
            return RedirectToAction("Index", "Home");
        }

        //show registration page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        //register new user
        [HttpPost]
        
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // check if the date from the model is valid
            if (ModelState.IsValid)
            {
                // create a new identity user object
                // pass in the incomming  model object to the IdentityUsr object
                // use the email as username field, and still use it as the password
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                // call the CreateAsyn method of the userManager 
                var result = await userManager.CreateAsync(user, model.Password);


                // check if user was successfully created
                if (result.Succeeded)
                {
                    // signij user using  SignInmanager
                    // ispersisten : false means we set session cookie, which is not persistent after browser is closed
                    await signInManager.SignInAsync(user, isPersistent: false);

                    // redirect user to "action" of "controller"
                    return RedirectToAction("Index", "Home");
                }
                
                // if not success -- loop through Errors collection
                // add each error to the modelstate object
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    // the errors are displayed in the view via the asp-validation-summary tag helper

                }
            }

            // rerender the page with error message if error occurs
            return View(model); // pass in the model object
        }



        //Login functionality

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // check if the date from the model is valid
            if (ModelState.IsValid)
            {
                
                // signin user using signManager PasswordsignAsync method which takes as parameter:
                // username, password, persistence (RememberMe in this case), account lockout

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);


                // check if user was successfully created
                if (result.Succeeded)
                {
                    // redirect to view based on rold -- todo next
                    // redirect user to "action" of "controller"
                    return RedirectToAction("Index", "Home");
                }

                // if errors exist -- add errors to model state
                ModelState.AddModelError(string.Empty, "Wrong username or password");
            }

            // rerender the page with error message if error occurs
            return View(model); // pass in the model object
        } 


        //renders file upload page for user
        [HttpGet]
        [Authorize]
         // user must be logged in to access this page
        public IActionResult FileForm()
        {
            return View();
        }
    }
}
