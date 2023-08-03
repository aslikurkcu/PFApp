using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFApp.Models;

namespace PFApp.Contexts
{
    public class InvestmentsContext : DbContext
    {

        public InvestmentsContext(DbContextOptions<InvestmentsContext> options) : base(options)
        {

        }

        public DbSet<Investment> Investments { get; set; }

    }
    
}