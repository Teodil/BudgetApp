using BudgetApp.Models.DTO;
using BudgetApp.Models.Utilitis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DataSource DataSource { get; set; }
        [PoleDescription(name: "Номер карты", isRequired: false)]
        public string? CardNumber { get; set; }
        [PoleDescription(name: "Описание", isRequired: false)]
        public string? Description { get; set; }
        [PoleDescription(name: "Сумма")]
        public decimal Summ {  get; set; }
        [PoleDescription(name: "Категория операции")]
        public OperationCategory OperationCategory { get; set; }
        [PoleDescription(name: "Тип операции", isRequired:false)]
        public OperationType? OperationType { get; set; }
        [PoleDescription(name: "Кэшбэк",isRequired:false)]
        public decimal CashBack { get; set; }


        public CardOperation(UploadDataDTO uploadDataDTO)
        {
            Date = uploadDataDTO.Date;
            DataSource = uploadDataDTO.DataSource;
            CardNumber = uploadDataDTO.CardNumber;
            Description = uploadDataDTO.Description;
            Summ = uploadDataDTO.Summ;
            OperationCategory = new OperationCategory();
            OperationCategory.Name = uploadDataDTO.OperationCategory;
            CashBack = uploadDataDTO.CashBack;
        }

        public CardOperation()
        {

        }

        public CardOperation(int id, DateTime date, DataSource dataSource, string? cardNumber, string? description, decimal summ, OperationCategory operationCategory, OperationType operationType, decimal cashBack)
        {
            Id = id;
            Date = date;
            DataSource = dataSource;
            CardNumber = cardNumber;
            Description = description;
            Summ = summ;
            OperationCategory = operationCategory;
            OperationType = operationType;
            CashBack = cashBack;
        }
    }
}
