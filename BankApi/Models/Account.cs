using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    
    }
}
