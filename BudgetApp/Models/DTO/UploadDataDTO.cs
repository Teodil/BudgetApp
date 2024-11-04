using BudgetApp.Models.Data;
using BudgetApp.Models.Utilitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.DTO
{
    public class UploadDataDTO
    {
        [PoleDescription(name: "Дата операции")]
        public DateTime Date { get; set; }

        [PoleDescription(name: "Номер карты")]
        public string? CardNumber { get; set; }
        [PoleDescription(name: "Описание")]
        public string? Description { get; set; }
        [PoleDescription(name: "Сумма")]
        public decimal Summ { get; set; }
        [PoleDescription(name: "Категория операции")]
        public string OperationCategory { get; set; }
        [PoleDescription(name: "Тип операции")]
        public string OperationType { get; set; }
        [PoleDescription(name: "Кэшбэк")]
        public decimal CashBack { get; set; }

        public DataSource DataSource { get; set; }
    }
}
