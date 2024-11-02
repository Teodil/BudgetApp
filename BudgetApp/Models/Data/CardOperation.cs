using BudgetApp.Models.Utilitis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.Data
{
    public class CardOperation
    {
        [PoleDescription(name: "Id", isEditable: false)]
        public int Id { get; set; }
        [PoleDescription(name: "Дата операции")]
        public DateTime Date { get; set; }
        [PoleDescription(name: "Банк")]
        public Bank Bank { get; set; }
        [PoleDescription(name: "Номер карты")]
        public string? CardNumber { get; set; }
        [PoleDescription(name: "Описание")]
        public string? Description { get; set; }
        [PoleDescription(name: "Сумма")]
        public decimal Summ {  get; set; }
        [PoleDescription(name: "Категория операции")]
        public OperationCategory OperationCategory { get; set; }
        [PoleDescription(name: "Тип операции")]
        public OperationType OperationType { get; set; }
        [PoleDescription(name: "Кэшбэк")]
        public decimal CashBack { get; set; }

    }
}
