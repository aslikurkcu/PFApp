using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFApp.DTO
{
    public class BillDTO
    {
        public string bill_Type { get; set; }
        public int amount { get; set; }
        public Boolean paid { get; set; }
        public int bill_id { get; set; }
        
    }
}