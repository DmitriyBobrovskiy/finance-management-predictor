using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_management_backend.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int CounterpartId { get; set; }
        public Counterpart Counterpart { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public int? LoanTypeId { get; set; }
        public LoanType LoanType { get; set; }
    }
}
