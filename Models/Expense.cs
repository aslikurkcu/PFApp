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
        public int Expense_Id { get; set; }
        public int User_Id { get; set; }
        public string Expense_Type { get; set; }
        public int Amount { get; set; }
        public DateTime Expense_Date { get; set; }
    }
}