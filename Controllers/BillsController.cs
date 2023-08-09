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
//using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;



namespace PFApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {

        private readonly BillsContext _dbContext;

        public BillsController(BillsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("getbills")]
        public async Task<ActionResult> GetBills(int user_id){

            var bills = await _dbContext.Bills
            .Where(entry => entry.user_Id == user_id)
            .Select(b => new BillDTO(){
                bill_Type = b.bill_Type,
                amount = b.amount,
                paid = b.paid,
                bill_id = b.bill_id
            })
            .ToListAsync();
            return Ok(bills);
        }

        [HttpGet("GetPaidBills")]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetPaidBills(int user_id){

            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var expenses = await _dbContext.Bills
            .Where(entry => entry.user_Id == user_id && entry.payment_date >= firstDayOfMonth 
            && entry.payment_date <= lastDayOfMonth && entry.paid == true)
            .Select(b => new ExpenseDTO(){
                expense_type  = "bill",
                amount = b.amount
            })
            .ToListAsync();
            return Ok(expenses);
        }


        [HttpPut("UpdateBill")]
        public async Task<ActionResult> UpdateBill(JsonElement data ){

        int Bill_id = Convert.ToInt32(data.GetProperty("Bill_id").ToString());
        bool Paid = Convert.ToBoolean(data.GetProperty("Paid").ToString());
        
        var bill = await _dbContext.Bills.FindAsync(Bill_id);
         
        if (bill == null)
        {
            return NotFound("dfghj");
        } 
        var parameters = new[]
        {
            new SqlParameter("@billId", SqlDbType.Int) { Value = bill.bill_id },
            new SqlParameter("@isPaid", SqlDbType.Bit) { Value = Paid }
        };

        await _dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateBillPaidStatus @billId, @isPaid", parameters);

        return NoContent();
        }
    

    [HttpPut("InsertBill")]
    public async Task<ActionResult> InsertBill(JsonElement data){

        int user_id = Convert.ToInt32(data.GetProperty("user_id").ToString());
        string bill_type = data.GetProperty("bill_type").ToString();
        int amount = Convert.ToInt32(data.GetProperty("amount").ToString());
       
        var parameters = new[]
        {
            new SqlParameter("@UserId", SqlDbType.Int) { Value = user_id },
            new SqlParameter("@BillType", SqlDbType.VarChar) { Value = bill_type},
            new SqlParameter("@Amount", SqlDbType.Int) { Value = amount }
        };


        await _dbContext.Database.ExecuteSqlRawAsync("EXEC InsertBill @UserId, @BillType, @Amount", parameters);
        
        return Ok();
    }


    }
}