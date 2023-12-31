using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PFApp.Models
{
    public class Expense
    {
        [Key]
        public int expense_id { get; set; }
        public int user_id { get; set; }
        public string expense_type { get; set; }
        public int amount { get; set; }
        public DateTime expense_date { get; set; }
    }
}