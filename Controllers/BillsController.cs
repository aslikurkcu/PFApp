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
    public class BillsController : ControllerBase
    {

        private readonly BillsContext _dbContext;

        public BillsController(BillsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBills()
        {
            if (_dbContext.Bills == null)
            {
                return NotFound();
            }
            return await _dbContext.Bills.ToListAsync();
        }

        [HttpGet("{bill_id}")]
        public async Task<ActionResult<Bill>> GetBills(int bill_id)
        {
            if (_dbContext.Bills == null)
            {
                return NotFound();
            }

            var bill = await _dbContext.Bills.FindAsync(bill_id);
            if (bill == null)
            {
                return NotFound();
            }
            return bill;
        }

    }
}