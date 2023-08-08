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
        string bill_type = data.GetProperty("expense_type").ToString();
        int amount = Convert.ToInt32(data.GetProperty("amount").ToString());
       
        var parameters = new[]
        {
            new SqlParameter("@UserId", SqlDbType.Int) { Value = user_id },
            new SqlParameter("@ExpenseType", SqlDbType.VarChar) { Value = bill_type},
            new SqlParameter("@Amount", SqlDbType.Int) { Value = amount }
        };


        await _dbContext.Database.ExecuteSqlRawAsync("EXEC InsertBill @UserId, @ExpenseType, @Amount", parameters);
        
        return Ok();
    }






/* 
        [HttpGet("expenses")]
        public async Task<ActionResult<IEnumerable<Expense>>> Getexpenses()
        {
            if (_dbContext.Expenses == null)
            {
                return NotFound();
            }
            return await _dbContext.Expenses.ToListAsync();
        } */


       /*  [HttpGet("getexpenses")]
        public async Task<ActionResult> GetExpenses(int user_id)
{
        var parameters = new SqlParameter("@userId", SqlDbType.Int) { Value = user_id };

        var expenses = await _dbContext.Expenses
            .FromSqlRaw("EXEC GetCurrentMonthExpensesByType @userId", parameters)
           
            .ToListAsync();

        return Ok(expenses);
} */

 /*.Select(e => new ExpenseDTO(){
                expense_type = e.expense_type,
                amount = e.amount,
                expense_id = e.expense_id
            })*/

        /* [HttpGet("userid")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(int user_id)
        {
            if (_dbContext.Expenses == null)
            {
                return NotFound();
            }
        
            return await _dbContext.Expenses.Where(entry => entry.User_Id == user_id).ToListAsync();
        
        } */
    }
}