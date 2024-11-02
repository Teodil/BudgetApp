using BudgetApp.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Infrastructure.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<CardOperation> CardOperations { get; set; }
        public DbSet <OperationType> OperationTypes { get; set; }
        public DbSet<OperationCategory> OperationCategories { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> dbContextOptions)
        : base(dbContextOptions)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            Bank bank1 = new Bank()
            {
                Id = 1,
                Name = "Т-банк"
            };
            Bank bank2 = new Bank()
            {
                Id = 2,
                Name = "Альфа банк"
            };
            this.Banks.Add(bank1);
            this.Banks.Add(bank2);
            OperationType type1 = new OperationType()
            {
                Id = 1,
                Name = "Списание"
            };
            OperationType type2 = new OperationType()
            {
                Id = 2,
                Name = "Пополнение"
            };
            this.OperationTypes.AddRange(type1, type2);

            this.SaveChanges();
            
        }
    }
}
