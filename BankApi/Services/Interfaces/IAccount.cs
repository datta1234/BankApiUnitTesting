using BankApi.Dtos;
using BankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BankApi.Services.Interfaces
{
    public interface IAccount
    {
        Task<decimal> GetBalanceByAccount(int AccountId);
        Task<List<FinancialTransaction>> GetTransactionsByAccount(int AccountId);
        Task<int> CreateAccount( int CustomerId);
        Task<int> CreateTransaction(TransactionDto ft);
        Task<bool> Transfer(TransferDto transferDto);
    }
}
