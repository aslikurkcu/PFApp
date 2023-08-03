using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFApp.Models;

namespace PFApp.Contexts
{
    public class BillsContext : DbContext
    {

        public BillsContext(DbContextOptions<BillsContext> options) : base(options)
        {

        }

        public DbSet<Bill> Bills { get; set; }

    }
    
}