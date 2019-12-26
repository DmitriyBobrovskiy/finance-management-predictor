using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_management_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace finance_management_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TransactionsController(DatabaseContext context)
        {
            _context = context;
            context.Fill();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> Get([FromQuery] object filter)
        {
            var result = _context.Transactions
                .Include(transaction => transaction.Category)
                    .ThenInclude(category => category.Icon)
                .Include(transaction => transaction.Counterpart)
                .Include(transaction => transaction.LoanType)
                .Include(transaction => transaction.TransactionType)
                .Include(transaction => transaction.Account)
                    .ThenInclude(account => account.AccountType)
                .Include(transaction => transaction.Account)
                    .ThenInclude(account => account.Currency);

            return Ok(result);
        }
    }
}