using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PFApp.Models
{
    public class Bill
    {
        [Key]
        public int Bill_Id { get; set; }
        public int User_Id { get; set; }
        public string Bill_Type { get; set; }
        public int Amount { get; set; }
        public Boolean Paid { get; set; }
        public Nullable<DateTime> Payment_Date { get; set; }
    }
}