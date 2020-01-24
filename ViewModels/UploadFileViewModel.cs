using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerApp.ViewModels
{
    public class UploadFileViewModel
    {
        //database field for file upload
        [Key]
        public int FileId { get; set; }

        [Required]
        public string UploaderName { get; set; }// name of the uploader

        [Required]
        public IFormFile UploadedFile { get; set; } // file path
        
        [Required]
        public string Description { get; set; } // description of the file

    }
}
