using BankApi.Db;
using BankApi.Models;
using BankApi.Services;
using BankApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace BankApiUnitTest
{
    public class AccountServiceTest
    {
        private readonly dbContext _dbContext;
        private readonly IAccount _accountService;
      
        public AccountServiceTest()
        {
            _dbContext = new InMemoryDbContextFactory().GetBankDbContext();
            _accountService = new AccountService(_dbContext);
         
        }

        [Fact]
        public async Task GetBalanceByAccount_Should_Return_Correct_Balance()
        {


            // Arrange
            var cust = _dbContext.Customer.OrderByDescending(u => u.Id).FirstOrDefault();
            var customer = new Customer
            {
                Id = cust==null?1: cust.Id+1,
               FirstName = "test",
                LastName="Test",
                
            };

            _dbContext.Customer.Add(customer);
            _dbContext.SaveChanges();

           var acc = _dbContext.Account.OrderByDescending(u => u.Id).FirstOrDefault();
            var account = new Account
            {
                Id = acc == null ? 1 : acc.Id + 1,
                CustomerId = customer.Id,
                AccountBalance = 900
            };

            _dbContext.Account.Add(account);
            _dbContext.SaveChanges();

            // Act
            var actual = await _accountService.GetBalanceByAccount(account.Id);

          
            // Assert
            Assert.Equal(account.AccountBalance, actual);
        }

        [Fact]
        public async Task GetBalanceByAccount_Should_Return_Exception_IfNotExist()
        {
            // Act
            var ex = await Assert.ThrowsAsync<HttpResponseException>(() => _accountService.GetBalanceByAccount(0));
            // Assert
            Assert.Equal("Account Not Found", ex.Response.ReasonPhrase);
        }
        [Fact]
        public async Task GetTransactionsByAccount_Should_Return_Correct_transactions()
        {
            
            // Arrange
            var cust = _dbContext.Customer.OrderByDescending(u => u.Id).FirstOrDefault();
            var customer = new Customer
            {
                Id = cust == null ? 1 : cust.Id + 1,
                FirstName = "test",
                LastName = "Test",

            };

            _dbContext.Customer.Add(customer);
            _dbContext.SaveChanges();

            var acc = _dbContext.Account.OrderByDescending(u => u.Id).FirstOrDefault();
            var account = new Account
            {
                Id = acc == null ? 1 : acc.Id + 1,
                CustomerId = customer.Id,
                AccountBalance = 900
            };

            _dbContext.Account.Add(account);
            _dbContext.SaveChanges();


            var tran = _dbContext.FinancialTransaction.OrderByDescending(u => u.Id).FirstOrDefault();
            List<FinancialTransaction> fts = new List<FinancialTransaction>();
        var  ft = new FinancialTransaction
            {
                Id = tran == null ? 1 : tran.Id + 1,
                FinancialAccountId = account.Id,
                CustomerId = customer.Id,
                Amount = 900,
                TransactionType = 1,
                DateCreated = DateTime.Now,
                RunningTotal = 900,

            };
            _dbContext.FinancialTransaction.Add(ft);
            _dbContext.SaveChanges();
            // Act
            var actual = await _accountService.GetTransactionsByAccount(account.Id);
      
            // Assert
            Assert.Collection(actual, item => Assert.Equal(900, item.RunningTotal));
        }
        [Fact]
        public async Task GetTransactionsByAccount_Should_Return_Exception_IfAccountNotExist()
        {
            // Act
            var ex = await Assert.ThrowsAsync<HttpResponseException>(() => _accountService.GetTransactionsByAccount(0));
            // Assert
            Assert.Equal("Account Not Found", ex.Response.ReasonPhrase);
        }

        [Fact]
        public async Task CreateAccount_Should_Return_Correct_Account_Number()
        {
            // Arrange
            var cust = _dbContext.Customer.OrderByDescending(u => u.Id).FirstOrDefault();
            var customer = new Customer
            {
                Id = cust == null ? 1 : cust.Id + 1,
                FirstName = "test",
                LastName = "Test",

            };

            _dbContext.Customer.Add(customer);
            _dbContext.SaveChanges();
            // Act
            var acc = _dbContext.Account.OrderByDescending(u => u.Id).FirstOrDefault();
            var actual = await _accountService.CreateAccount(customer.Id);

             // Assert
            Assert.Equal(acc == null ? 1 : acc.Id + 1, actual);
        }

        [Fact]
        public async Task CreateAccount_Should_Return_Exception_IfCustomerNotExist()
        {
            // Act
            var ex = await Assert.ThrowsAsync<HttpResponseException>(() => _accountService.CreateAccount(0));
            // Assert
            Assert.Equal("Customer Not Found", ex.Response.ReasonPhrase);
        }

    }
}
