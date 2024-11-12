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

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using System.IO;
using Npoi.Mapper;
using NPOI.OpenXml4Net.OPC;
using NPOI.HSSF.UserModel;
using MathNet.Numerics.Distributions;
using System.Reflection;
using System.ComponentModel;


namespace BudgetApp.Services
{

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime BirthDay { get; set; }
    }

    public class Parser
    {

        private Repository _repository;
        private EventTransferService _notifyService;

        public Parser(Repository repository, EventTransferService notifyService)
        {
            _repository = repository;
            _notifyService = notifyService;
        }

        public List<UploadDataDTO> Parse(string filePath, DataSource dataSource)
        {
            IWorkbook workbook;
            using(FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                if (filePath.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    workbook = new XSSFWorkbook(stream);
                }
            }


            int test = workbook.NumberOfSheets;
            // Получение листа
            ISheet sheet = workbook.GetSheetAt(0);

            Dictionary<int, PoleAccordance> coloumsIDs = new Dictionary<int, PoleAccordance>();
            List<UploadDataDTO> data = new List<UploadDataDTO>();
            for (int rowIndex = 0; rowIndex < sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                if (rowIndex == 0)
                {
                    for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
                    {
                        ICell cell = row.GetCell(cellIndex);
                        PoleAccordance? poleAccordance = dataSource.PoleAccordances.FirstOrDefault(x => x.ExcelColumnName == cell.StringCellValue);
                        if (poleAccordance != null)
                        {
                            coloumsIDs.Add(cellIndex, poleAccordance);
                        }
                        
                    }
                }
                else
                {
                    UploadDataDTO uploadDataDTO = new UploadDataDTO();
                    uploadDataDTO.IsDuplicate = false;
                    foreach (var key in coloumsIDs)
                    {
                        PropertyInfo? prop = uploadDataDTO.GetType().GetProperty(key.Value.PoleName, BindingFlags.Public | BindingFlags.Instance);
                        if(prop != null)
                        {
                            ICell cell = row.GetCell(key.Key);
                            if (cell == null)
                                continue;
                            string cellValue = cell.CellType == CellType.Numeric ? Convert.ToString(cell.NumericCellValue) : cell.StringCellValue;
                            dynamic value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromString(cellValue);
                            prop.SetValue(uploadDataDTO, value);
                        }
                    }
                    uploadDataDTO.DataSource = dataSource;
                    if(_repository.IsCardOperationExist(uploadDataDTO))
                        uploadDataDTO.IsDuplicate = true;
                    data.Add(uploadDataDTO);
                }
                
            }

            return data;

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
