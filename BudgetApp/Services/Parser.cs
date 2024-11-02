using BudgetApp.Infrastructure.Context;
using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.Models.DTO;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetApp.Services
{
    public class Parser
    {

        private Repository _repository;
        private NotifyService _notifyService;

        public Parser(Repository repository, NotifyService notifyService)
        {
            _repository = repository;
            _notifyService = notifyService;
        }

        public ParserResultDTO Parse(string filePath, Bank bank)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook;
            Worksheet worksheet;

            workbook = excel.Workbooks.Open(filePath);
            worksheet = workbook.Worksheets[1];

            Dictionary<string, int> columnNames = new Dictionary<string,int>();

            bool readRow = true;
            int colIndex = 1;
            int gap = 0;

            while (readRow)
            {
                Microsoft.Office.Interop.Excel.Range cell = worksheet.Cells[1, colIndex];
                string value = Convert.ToString(cell.Value);
                if (System.String.IsNullOrEmpty(value))
                {
                    gap += 1;
                    if (gap > 4)
                    {
                        readRow = false;
                    }
                }
                else
                {
                    columnNames.Add(value,colIndex);
                }
                colIndex += 1;
            }

            List<OperationCategory> operationCategories = ParseCategories(columnNames["Категория"],worksheet);

            readRow = true;
            int rowIndex = 2;
            List<CardOperation> operations = new List<CardOperation>();


            while (readRow)
            {
                if (System.String.IsNullOrEmpty(worksheet.Cells[rowIndex, 1].Value))
                {
                    readRow = false;
                    continue;
                }
                CardOperation cardOperation = new CardOperation()
                {
                    Bank = _repository.GetBank(1),
                    CardNumber = worksheet.Cells[rowIndex, columnNames["Номер карты"]].Value,
                    CashBack = Convert.ToDecimal(worksheet.Cells[rowIndex, columnNames["Кэшбэк"]].Value),
                    Date = Convert.ToDateTime(worksheet.Cells[rowIndex, columnNames["Дата операции"]].Value),
                    Description = worksheet.Cells[rowIndex, columnNames[columnNames.Keys.First(x=>x.Contains("Описание"))]].Value,
                    Summ = Convert.ToDecimal(worksheet.Cells[rowIndex, columnNames["Сумма платежа"]].Value),
                    OperationCategory = _repository.IsOperationCategoryExist(new OperationCategory { Name = worksheet.Cells[rowIndex, columnNames["Категория"]].Value }) 
                                                            ? _repository.GetOperationCategoryByName(worksheet.Cells[rowIndex, columnNames["Категория"]].Value) 
                                                            : new OperationCategory { Name = worksheet.Cells[rowIndex, columnNames["Категория"]].Value},
                    OperationType = _repository.GetOperationTypeBySumm(Convert.ToDecimal(worksheet.Cells[rowIndex, columnNames["Сумма платежа"]].Value))
                    
                };
                if(!_repository.IsCardOperationExist(cardOperation))
                {
                    operations.Add(cardOperation);
                }
                rowIndex++;
            }


            workbook.Close(0);
            excel.Quit();
            return new ParserResultDTO(operations, operationCategories);
            //_repository.AddRangeCardOperation(operations);
            //_notifyService.DataParsed();
        }
        
        public List<OperationCategory> ParseCategories(int categoryColIndex, Worksheet worksheet)
        {
            List<OperationCategory> categories = new List<OperationCategory>();
            bool readRow = true;
            int rowIndex = 2;
            while (readRow)
            {
                if (System.String.IsNullOrEmpty(worksheet.Cells[rowIndex, 1].Value))
                {
                    readRow = false;
                    continue;
                }

                string value = worksheet.Cells[rowIndex, categoryColIndex].Value;
                OperationCategory operationCategory = new OperationCategory { Name = value };
                if (categories.Exists(x=>x.Name == value) == false && _repository.IsOperationCategoryExist(operationCategory) == false)
                {
                    categories.Add(operationCategory);
                }

                rowIndex++;
            }

            return categories; //_repository.AddRangeOperationCategory(categories);
        }
    }
}
