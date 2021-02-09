using BankApi.Db;
using BankApi.Dtos;
using BankApi.Enum;
using BankApi.Models;
using BankApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BankApi.Services
{
    public class CustomerService : ICustomer
    {
        private dbContext _dbContext;
        private IAccount _accountService;
        public CustomerService(dbContext dbContext, IAccount accountservice)
        {
            _dbContext = dbContext;
            _accountService = accountservice;
        }
        public async Task<Customer> GetbyId(int CustomerId)
        {
            return await _dbContext.Customer.FirstOrDefaultAsync(x => x.Id== CustomerId);
        }
        public async Task<int> CreateUpdateCustomer(CustomerDto customerDto)
        {
            if (customerDto.InitialDeposit < 0)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid initial deposit" });
            Customer customer=  new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    DateCreated =DateTime.Now
                };
                  await _dbContext.Customer.AddAsync(customer);
                  await _dbContext.SaveChangesAsync();
                  var accountId = await _accountService.CreateAccount(customer.Id);
             if(customerDto.InitialDeposit>0)
                {
                    TransactionDto ft = new TransactionDto();
                    ft.FinancialAccountId = accountId;
                    ft.CustomerId = customer.Id;
                    ft.Amount = customerDto.InitialDeposit;
                    ft.type = Common.TransactionType.Credit;
                    ft.Description = "Initial Deposite";
                    var transactionId = await _accountService.CreateTransaction(ft);
                }
        
                return customer.Id;
            
        }
    }
}
