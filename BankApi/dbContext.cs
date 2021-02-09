using BankApi.Models;
using Microsoft.EntityFrameworkCore;
namespace BankApi.Db
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<FinancialTransaction> FinancialTransaction { get; set; }
      
    }
}
