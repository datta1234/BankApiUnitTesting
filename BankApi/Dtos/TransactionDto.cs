using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApi.Enum;
using BankApi.Models;

namespace BankApi.Dtos
{
    public class TransactionDto
    {
      
        public int FinancialAccountId { get; set; }
        public int CustomerId { get; set; }
        public Common.TransactionType type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
