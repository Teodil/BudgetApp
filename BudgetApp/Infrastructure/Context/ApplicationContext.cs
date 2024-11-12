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
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<CardOperation> CardOperations { get; set; }
        public DbSet <OperationType> OperationTypes { get; set; }
        public DbSet<OperationCategory> OperationCategories { get; set; }
        public DbSet<PoleAccordance> PoleAccordances { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> dbContextOptions)
        : base(dbContextOptions)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            DataSource bank1 = new DataSource()
            {
                Id = 1,
                Name = "Т-банк"
            };
            DataSource bank2 = new DataSource()
            {
                Id = 2,
                Name = "Альфа банк"
            };
            this.DataSources.Add(bank1);
            this.DataSources.Add(bank2);
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

            List<PoleAccordance> poleAccordances = new List<PoleAccordance>();
            PoleAccordance poleAccordance1 = new PoleAccordance();
            poleAccordance1.MapPole("Date", "Дата операции", "Дата операции");
            PoleAccordance poleAccordance2 = new PoleAccordance();
            poleAccordance2.MapPole("CardNumber", "Номер карты", "Номер карты");
            PoleAccordance poleAccordance3 = new PoleAccordance();
            poleAccordance3.MapPole("Summ", "Сумма операции", "Сумма операции");
            PoleAccordance poleAccordance4 = new PoleAccordance();
            poleAccordance4.MapPole("CashBack", "Кэшбэк", "Кэшбэк");
            PoleAccordance poleAccordance5 = new PoleAccordance();
            poleAccordance5.MapPole("OperationCategory", "Категория", "Категория");
            PoleAccordance poleAccordance6 = new PoleAccordance();
            poleAccordance6.MapPole("Description", "Описание", "Описание");

            bank1.PoleAccordances = new List<PoleAccordance>();
            bank1.PoleAccordances.AddRange([poleAccordance1, poleAccordance2, poleAccordance3, poleAccordance4, poleAccordance5, poleAccordance6]);


            this.SaveChanges();
            
        }
    }
}
