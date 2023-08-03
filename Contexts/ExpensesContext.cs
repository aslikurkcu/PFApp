using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFApp.Models;

namespace PFApp.Contexts
{
    public class ExpensesContext :DbContext
    {

        public ExpensesContext(DbContextOptions<ExpensesContext> options) : base(options)
        {

        }

        public DbSet<Expense> Expenses { get; set; }

    }
}