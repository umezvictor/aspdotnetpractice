using FileManagerApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileManagerApp.Controllers
{
   /// [Produces("application/json")]
    [Route("api/files")]
    [ApiController]
    public class FilesApiController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserContext _context;

        public FilesApiController(IHostingEnvironment hostingEnvironment, UserContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FileUpload>> GetFiles()
        {
            return _context.Files;
        }

        //create new file
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "uploads";
                string webRoothPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRoothPath, folderName);

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if(file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }


                
                return Ok();
                
            }
            catch (System.Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
