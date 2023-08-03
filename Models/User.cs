using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PFApp.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public int Income { get; set; }
    }
}