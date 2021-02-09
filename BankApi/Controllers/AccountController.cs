using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApi.Dtos;
using BankApi.Models;
using BankApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount accountService;

        public AccountController(IAccount accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("{AccountId}/balance")]
        public async Task<decimal> GetBalanceByAccount(int AccountId)
        {
            return await accountService.GetBalanceByAccount(AccountId);
        }

        [HttpGet("{AccountId}/transactions")]

        public async Task<List<FinancialTransaction>> GetTransactionsByAccount(int AccountId)
        {
            return await accountService.GetTransactionsByAccount(AccountId);
        }

        [HttpPost]
        public async Task<int> CreateAccount(int customerId)
        {
            return await accountService.CreateAccount(customerId);
        }

        [HttpPost("transaction")]
        public async Task<int> CreateTransaction(TransactionDto transaction)
        {
            return await accountService.CreateTransaction(transaction);
        }

        [HttpPost("transfer")]
        public async Task<bool> Transfer(TransferDto transfer)
        {
            return await accountService.Transfer(transfer);
        }

    }
}