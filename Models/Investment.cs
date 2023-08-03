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
        public int Invest_Id { get; set; }
        public int User_Id { get; set; }
        public string Invest_Type { get; set; }
        public int Amount { get; set; }
        public DateTime Invest_Date { get; set; }
        
    }
}