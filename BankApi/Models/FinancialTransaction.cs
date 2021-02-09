using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Models
{
    public class FinancialTransaction
    {
        [Key]
        public int Id { get; set; }
        public int FinancialAccountId { get; set; }
        public int CustomerId { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal RunningTotal { get; set; }
        public string Description { get; set; }
        public int FromAccount  { get; set; }
        public int ToAccount { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
