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
using PFApp.DTO;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;


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


    [HttpPut("InsertExpense")]
    public async Task<ActionResult> InsertExpense(JsonElement data){

        int user_id = Convert.ToInt32(data.GetProperty("user_id").ToString());
        string expense_type = data.GetProperty("expense_type").ToString();
        int amount = Convert.ToInt32(data.GetProperty("amount").ToString());
       
        var parameters = new[]
        {
            new SqlParameter("@UserId", SqlDbType.Int) { Value = user_id },
            new SqlParameter("@ExpenseType", SqlDbType.VarChar) { Value = expense_type},
            new SqlParameter("@Amount", SqlDbType.Int) { Value = amount }
        };


        await _dbContext.Database.ExecuteSqlRawAsync("EXEC InsertExpense @UserId, @ExpenseType, @Amount", parameters);
        
        return Ok();
    }


    [HttpGet("GetExpenses")]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetExpenses(int user_id){

            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


            var expenses = await _dbContext.Expenses
            .Where(entry => entry.user_id == user_id && entry.expense_date >= firstDayOfMonth 
            && entry.expense_date <= lastDayOfMonth)
            .Select(e => new ExpenseDTO(){
                expense_type = e.expense_type,
                amount = e.amount,
            })
            .ToListAsync();
            return Ok(expenses);
    }

    [HttpGet("GetExpensesDaily")]
    public async Task<ActionResult<IEnumerable<int>>> GetExpensesDaily(int user_id)
    {
        var today = DateTime.Today;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var expenses = new List<int>();

        for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
        {
            var totalExpenses = await _dbContext.Expenses
                .Where(entry => entry.user_id == user_id  && entry.expense_date.Date == date.Date )
                .SumAsync(e => e.amount);

            expenses.Add(totalExpenses);
        }
        return Ok(expenses);
    }

    [HttpGet("GetExpensesMonthly")]
    public async Task<ActionResult<List<int>>> GetExpensesYearlyMonthly(int user_id)
    {
        var today = DateTime.Today;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var endOfYear = new DateTime(today.Year, 12, 31);

        var yearlyTotalExpenses = new List<int>();

        for (int month = 1; month <= 12; month++)
        {
            var firstDayOfMonth = new DateTime(today.Year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var totalExpensesForMonth = await _dbContext.Expenses
                .Where(entry => entry.user_id == user_id && entry.expense_date >= firstDayOfMonth && entry.expense_date <= lastDayOfMonth)
                .SumAsync(e => e.amount);

            yearlyTotalExpenses.Add(totalExpensesForMonth);
        }

        return Ok(yearlyTotalExpenses);
    }




    }
    
}