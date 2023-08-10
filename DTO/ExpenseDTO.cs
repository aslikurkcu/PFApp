using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFApp.DTO
{
    public class ExpenseDTO
    {
        public string expense_type { get; set; }
        public int amount { get; set; }
        public DateTime expense_date { get; set; }
    }
}