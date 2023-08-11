using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PFApp.Models;

namespace PFApp.Contexts
{
    public class UsersContext : IdentityDbContext<User,Role,int>
    {
         public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {

        }

        public DbSet<User> AspNetUsers { get; set; }

    }
}