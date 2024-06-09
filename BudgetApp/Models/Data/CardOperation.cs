using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.Data
{
    public class CardOperation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Bank Bank { get; set; }
        public string CardNumber { get; set; }
        public string Description { get; set; }
        public decimal Summ {  get; set; }
        public OperationType OperationType { get; set; }
        public decimal CashBack { get; set; }
    }
}
