using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApi.Dtos;
using BankApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer customerService;
 
        public CustomerController(ICustomer customerService)
        {
            this.customerService = customerService;
        }

       [HttpPost]
        public async Task<int> CreateCustomer(CustomerDto customerDto)
        {
           return await customerService.CreateUpdateCustomer(customerDto);
        }
    }
}