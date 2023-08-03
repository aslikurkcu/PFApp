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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Investment>>> GetInvestments()
        {
            if (_dbContext.Investments == null)
            {
                return NotFound();
            }
            return await _dbContext.Investments.ToListAsync();
        }

        [HttpGet("{invest_id}")]
        public async Task<ActionResult<Investment>> GetInvestments(int invest_id)
        {
            if (_dbContext.Investments == null)
            {
                return NotFound();
            }

            var invest = await _dbContext.Investments.FindAsync(invest_id);
            if (invest == null)
            {
                return NotFound();
            }
            return invest;
        }

    }
}