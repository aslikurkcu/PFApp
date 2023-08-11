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
    public class InvestmentsController : ControllerBase
    {
        private readonly InvestmentsContext _dbContext;

        public InvestmentsController(InvestmentsContext dbContext)
        {
            _dbContext = dbContext;
        }


      /* [HttpGet("GetInvestments")]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetInvestments(int user_id){

            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


            var investments = await _dbContext.Investments
            .Where(entry => entry.user_id == user_id && entry.invest_date >= firstDayOfMonth 
            && entry.invest_date <= lastDayOfMonth)
            .Select(e => new ExpenseDTO(){
                expense_type = e.expense_type,
                amount = e.amount,
            })
            .ToListAsync();
            return Ok(expenses);
    } */

    [HttpPut("InsertInvest")]
    public async Task<ActionResult> InsertInvest(JsonElement data){

        int user_id = Convert.ToInt32(data.GetProperty("user_id").ToString());
        string invest_type = data.GetProperty("invest_type").ToString();
        int amount = Convert.ToInt32(data.GetProperty("amount").ToString());
       
        var parameters = new[]
        {
            new SqlParameter("@UserId", SqlDbType.Int) { Value = user_id },
            new SqlParameter("@InvestType", SqlDbType.VarChar) { Value = invest_type},
            new SqlParameter("@Amount", SqlDbType.Int) { Value = amount }
        };


        await _dbContext.Database.ExecuteSqlRawAsync("EXEC InsertInvestment @UserId, @InvestType, @Amount", parameters);
        
        return Ok();
    }

    [HttpGet("GetInvestmentsDaily")]
    public async Task<ActionResult<IEnumerable<int>>> GetInvestmentsDayily(int user_id)
    {

        var today = DateTime.Today;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var investments = new List<int>();

        for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
        {
            var totalInvestments = await _dbContext.Investments
                .Where(entry => entry.user_id == user_id  && entry.invest_date.Date == date.Date)
                .SumAsync(i => i.amount);

            investments.Add(totalInvestments);
        }

    return Ok(investments);
    }


    [HttpGet("GetInvestmentsDailyByType")]
    public async Task<ActionResult<Dictionary<string, List<int>>>> GetInvestmentsDailyByType(int user_id)
    {
        var today = DateTime.Today;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var investmentTypes = new List<string> { "dollar", "euro", "XAU" };
        var investmentsByType = new Dictionary<string, List<int>>();

        foreach (var investmentType in investmentTypes)
        {
            var investments = new List<int>();

            for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                var totalInvestments = await _dbContext.Investments
                    .Where(entry => entry.user_id == user_id && entry.invest_date.Date == date.Date && entry.invest_type == investmentType)
                    .SumAsync(i => i.amount);

                investments.Add(totalInvestments);
            }

        investmentsByType.Add(investmentType, investments);
        }

    return Ok(investmentsByType);
    }

    [HttpGet("GetInvestmentsMonthly")]
    public async Task<ActionResult<List<int>>> GetInvestmentsMonthly(int user_id)
    {
        var today = DateTime.Today;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var endOfYear = new DateTime(today.Year, 12, 31);

        var yearlyTotalInvestments = new List<int>();

        for (DateTime month = startOfYear; month <= endOfYear; month = month.AddMonths(1))
        {
            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var totalInvestmentsForMonth = await _dbContext.Investments
                .Where(entry => entry.user_id == user_id && entry.invest_date >= firstDayOfMonth && entry.invest_date <= lastDayOfMonth)
                .SumAsync(i => i.amount);

            yearlyTotalInvestments.Add(totalInvestmentsForMonth);
        }

        return Ok(yearlyTotalInvestments);
    }





    }
}