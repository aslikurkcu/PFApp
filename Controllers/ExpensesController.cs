using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PFApp.Contexts;
using PFApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace PFApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {

        private readonly ExpensesContext _dbContext;

        public ExpensesController(ExpensesContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("expenses")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            if (_dbContext.Expenses == null)
            {
                return NotFound();
            }
            return await _dbContext.Expenses.ToListAsync();
        }

        [HttpGet("userid")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(int user_id)
        {
            if (_dbContext.Expenses == null)
            {
                return NotFound();
            }
        
            return await _dbContext.Expenses.Where(entry => entry.User_Id == user_id).ToListAsync();
        
        }
    }
}