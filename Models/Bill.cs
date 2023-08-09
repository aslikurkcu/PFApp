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
        public int bill_id { get; set; }
        public int user_Id { get; set; }
        public string bill_Type { get; set; }
        public int amount { get; set; }
        public Boolean paid { get; set; }
        public Nullable<DateTime> payment_date { get; set; }
    }
}