using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFApp.DTO
{
    public class UserRegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Income { get; set; }
    }
}