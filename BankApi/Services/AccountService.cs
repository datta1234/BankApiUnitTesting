using BankApi.Dtos;
using BankApi.Models;
using BankApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BankApi.Db;
using BankApi.Enum;

namespace BankApi.Services
{
    public class AccountService : IAccount
    {
        private dbContext _dbContext;
        public AccountService(dbContext db)
        {
            _dbContext = db;
        }

        public async Task<decimal> GetBalanceByAccount(int AccountId)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == AccountId);
            if (account == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Account Not Found" });
            return account.AccountBalance;
        }
        
        public async Task<List<FinancialTransaction>> GetTransactionsByAccount(int AccountId)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == AccountId);
            if (account == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Account Not Found" });
            var transactions = await _dbContext.FinancialTransaction.Where(x => x.FinancialAccountId == AccountId).ToListAsync();

            return transactions;
        }

        public async Task<int> CreateAccount(int customerId)
        {
            Customer customer = await _dbContext.Customer.FirstOrDefaultAsync(x => x.Id == customerId);
            if (customer == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Customer Not Found" });
               Account account = new Account
            {
                CustomerId = customerId,
                DateCreated = DateTime.Now,
                AccountBalance = 0
            };

            _dbContext.Account.Add(account);
            _dbContext.SaveChanges();
            return account.Id;
        }

        public async Task<int> CreateTransaction(TransactionDto ft)
        {
            var account = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == ft.FinancialAccountId);
            if (account == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Account Not Found" });
            if (ft.Amount <= 0)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Amount should be greater then 0.00" });
            if (ft.type != Common.TransactionType.Credit && ft.type != Common.TransactionType.Debit)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Transaction type is invalid" });
            int Id = 0;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                var financialTransaction = new FinancialTransaction
                {
                    CustomerId = ft.CustomerId,
                    FinancialAccountId = account.Id,
                    Amount = ft.Amount,
                    TransactionType = (int)ft.type,
                    RunningTotal = (int)ft.type == 1 ? (account.AccountBalance + ft.Amount) : (account.AccountBalance - ft.Amount),
                    DateCreated = DateTime.Now,
                    Description = ft.Description,
                };
                _dbContext.FinancialTransaction.Add(financialTransaction);
                _dbContext.SaveChanges();
                account.AccountBalance = financialTransaction.RunningTotal;
                account.DateUpdated = DateTime.Now;
                _dbContext.Account.Update(account);
                _dbContext.SaveChanges();
                await transaction.CommitAsync();
                Id = financialTransaction.Id;

            }
            return Id;
        }

        public async Task<bool> Transfer(TransferDto transfer)
        {
            var fromAccount = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == transfer.FromAccount);
            if (fromAccount == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Your account does not exist" });
            Account toAccount = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == transfer.ToAccount);
            if (toAccount == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Beneficiary account does not exist" });
            if (fromAccount.AccountBalance < transfer.Amount)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Insufficient account balance" });
            if (transfer.Amount <= 0)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Amount should be greater then 0.00" });
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                var fromTransaction = new FinancialTransaction
                {
                    CustomerId = fromAccount.CustomerId,
                    FinancialAccountId = fromAccount.Id,
                    Amount = transfer.Amount,
                    TransactionType = 2,
                    RunningTotal = (fromAccount.AccountBalance - transfer.Amount),
                    DateCreated = DateTime.Now,
                    Description = transfer.FromDecription,
                    ToAccount = transfer.ToAccount,
                };
                _dbContext.FinancialTransaction.Add(fromTransaction);
                _dbContext.SaveChanges();
                fromAccount.AccountBalance = fromTransaction.RunningTotal;
                fromAccount.DateUpdated = DateTime.Now;
                _dbContext.Account.Update(fromAccount);
                _dbContext.SaveChanges();

             var toTransaction = new FinancialTransaction
                {
                    CustomerId = toAccount.CustomerId,
                    FinancialAccountId = toAccount.Id,
                    Amount = transfer.Amount,
                    TransactionType = 1,
                    RunningTotal = (toAccount.AccountBalance + transfer.Amount),
                    DateCreated = DateTime.Now,
                    Description = transfer.ToDecription,
                    FromAccount = transfer.FromAccount
                };
                _dbContext.FinancialTransaction.Add(toTransaction);
                _dbContext.SaveChanges();
                toAccount.AccountBalance = toTransaction.RunningTotal;
                toAccount.DateUpdated = DateTime.Now;
                _dbContext.Account.Update(toAccount);
                _dbContext.SaveChanges();

                await transaction.CommitAsync();
            }
            return true;
        }
    }
}
