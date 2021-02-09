using BankApi.Dtos;
using BankApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankApi.Services.Interfaces
{
    public interface ICustomer
    {
        Task<Customer> GetbyId(int CustomerId);
        Task<int> CreateUpdateCustomer(CustomerDto customerDto);
    }
}
