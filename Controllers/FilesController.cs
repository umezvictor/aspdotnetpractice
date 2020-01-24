using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileManagerApp.ViewModels;
using FileManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileManagerApp.Controllers
{
    
    public class FilesController : Controller
    {
        private readonly UserContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        //inject IHostingservice inside the constructor

        public FilesController(UserContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }
        // display file upload page for users with the role 'User'
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult UploadFiles()
        {
            return View();
        }

        // Handle post request when file upload form is submitted
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadFiles(UploadFileViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //create variable to hold file name
                    string uniqueFileName = null;

                    //check if the user uploaded a file
                    //upload the file to the "uploads" directory in wwwroot folder
                    //to get the physical path, use IhostingEnvironment service via dependency injection
                    //using its constructor -- do it in this controller

                    if (model.UploadedFile != null)
                    {
                        //webrootpath gives us the absolute rootpath of wwwroot folder
                        //we need to combine it with the "uploads folder"
                        //we do this using Path class from system.io
                        //Combine methids returns the path as a string
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

                        //ensure there is no duplicate filename using GUId
                        //attach the filename using the FileName property of the uploaded file

                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedFile.FileName;

                        //combine unique filename with pathstring
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        //copy the file to the uploads folder in the server
                        model.UploadedFile.CopyTo(new FileStream(filePath, FileMode.Create));

                    }

                    //create a new FileUpload class Object -- instance of FileUpload.cs in model folder
                    //set the properties to that of the incoming model
                    FileUpload newFile = new FileUpload
                    {
                        UploaderName = model.UploaderName,
                        FilePath = uniqueFileName,
                        Description = model.Description
                    };

                    //save record
                    _context.Files.Add(newFile);//at this point we have the file and form fields
                    await _context.SaveChangesAsync();

                    return RedirectToAction("UploadFiles", "Files");
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "File could not be uploaded");  
            }
            return View(model);
        }



        // display page to view files for users with the role 'Admin'
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewFiles()
        {
            return View(await _context.Files.ToListAsync());
        }
   
    }
}
 