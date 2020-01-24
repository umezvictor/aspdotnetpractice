using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerApp.Models
{// this class provides all the Dbset properties needed to manage
    //needed to manage the identity tables
    // this file can be named AppDBContext - better option
    public class UserContext : IdentityDbContext
    {
        // constructor for this class
        // db provider such as sql, mysql, postgresql, etc are supported by efcore
        //dbcontext options contains the connection string, etc
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        
        //create a DbSet for each type in the project
        // will create tables named Users, Files when we run migrations 
        public DbSet<User> Users { get; set; }

        public DbSet<FileUpload> Files { get; set; } // name of table


    }
}
