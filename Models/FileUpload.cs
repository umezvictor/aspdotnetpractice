using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerApp.Models
{
    public class FileUpload
    {
        //database field for file upload
        [Key]
        public int FileId { get; set; }

        [Required]
        public string UploaderName { get; set; }// name of the uploader

        [Required]
        public string FilePath { get; set; } // file path
        //if you use IFormFile,you have to create a navigation property
        //because IFormFile is a complex object, which will complicate things
        //also, we don't want to store the propertis of the file in the db
        //hence, do that in the viewmodel
       [Required]
        public string Description { get; set; } // description of the file

    }
}
