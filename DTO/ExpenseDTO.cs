using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFApp.DTO
{
    public class ExpenseDTO
    {
        public int expense_id { get; set; }
        public string expense_type { get; set; }
        public int amount { get; set; }
    }
}