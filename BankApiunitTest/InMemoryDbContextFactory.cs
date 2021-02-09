using Microsoft.EntityFrameworkCore;
using BankApi.Db;
using System;
using System.Collections.Generic;
using System.Text;
using BankApi.Models;

namespace BankApiUnitTest
{
    public class InMemoryDbContextFactory
    {
        public dbContext GetBankDbContext()
        {
            var options = new DbContextOptionsBuilder<dbContext>()
                            .UseInMemoryDatabase(databaseName: "BankDatabase")
                            .Options;
            var dbContext = new dbContext(options);
           
            return dbContext;
        }
    }
}
