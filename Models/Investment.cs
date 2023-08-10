using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PFApp.Models
{
    public class Investment
    {
        [Key]
        public int invest_id { get; set; }
        public int user_id { get; set; }
        public string invest_type { get; set; }
        public int amount { get; set; }
        public DateTime invest_date { get; set; }
        
    }
}