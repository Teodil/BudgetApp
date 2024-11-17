using BudgetApp.Infrastructure.Context;
using BudgetApp.Models.Data;
using BudgetApp.Models.DTO;
using BudgetApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Infrastructure.Repository
{
    public class Repository
    {
        private ApplicationContext _applicationContext;
        private EventTransferService _notifyService;

        public Repository(ApplicationContext applicationContext, EventTransferService notifyService)
        {
            _applicationContext = applicationContext;
            _notifyService = notifyService;
        }

        #region CardOperation
        public List<CardOperation> GetCardOperations(DateTime? from = null,DateTime? to = null, OperationCategory? operationCategory = null, OperationType? operationType = null)
        {
            var query = _applicationContext.CardOperations.AsQueryable();
            if (from != null)
                query = query.Where(x => x.Date.Date >= from.Value.Date);
            if (to != null)
                query = query.Where(x => x.Date.Date <= to.Value.Date);
            if (operationCategory != null)
                query = query.Where(x => x.OperationCategory == operationCategory);
            if (operationType != null)
                query = query.Where(x => x.OperationType == operationType);

            return query.ToList();
        }

        public bool IsCardOperationExist(CardOperation operation)
        {
            return _applicationContext.CardOperations.Any(x=>x.Date == operation.Date && x.DataSource == operation.DataSource && x.Summ == operation.Summ);
        }

        public bool IsCardOperationExist(UploadDataDTO uploadDataDTO)
        {
            return _applicationContext.CardOperations.Any(x => x.Date == uploadDataDTO.Date 
                                                          && x.DataSource == uploadDataDTO.DataSource 
                                                          && x.Summ == uploadDataDTO.Summ);
        }

        public void AddCardOperation(CardOperation operation)
        {
            _applicationContext.CardOperations.Add(operation);
            _applicationContext.SaveChanges();
            _notifyService.DataBaseDataUpdated();
        }

        public void AddRangeCardOperation(IEnumerable<CardOperation> operations)
        {
            _applicationContext.CardOperations.AddRange(operations);
            _applicationContext.SaveChanges();
            _notifyService.DataBaseDataUpdated();
        }
        #endregion

        #region OperationCategory
        public bool IsOperationCategoryExist(OperationCategory category)
        {
            return _applicationContext.OperationCategories.Any(x => x.Name == category.Name);
        }

        public void AddOperationCategory(OperationCategory category)
        {
            _applicationContext.OperationCategories.Add(category);
            _applicationContext.SaveChanges();
            _notifyService.DataBaseDataUpdated();
        }

        public void AddRangeOperationCategory(IEnumerable<OperationCategory> categories)
        {
            _applicationContext.OperationCategories.AddRange(categories);
            _applicationContext.SaveChanges();
            _notifyService.DataBaseDataUpdated();
        }

        public OperationCategory? GetOperationCategoryByName(string name)
        {
            return _applicationContext.OperationCategories.FirstOrDefault(x=> x.Name == name);
        }

        public List<OperationCategory> GetOperationCategories()
        {
            return _applicationContext.OperationCategories.ToList();
        }
        #endregion

        #region DataSource
        public DataSource? GetDataSource(int id)
        {
            return _applicationContext.DataSources.FirstOrDefault(x => x.Id == id);
        }

        public List<DataSource> GetDataSources()
        {
            return _applicationContext.DataSources.ToList();
        }

        public void AddDataDataSources(DataSource dataSource)
        {
            _applicationContext.DataSources.Add(dataSource);
            _applicationContext.SaveChanges();
            _notifyService.DataBaseDataUpdated();
        }

        public void UpdateDataDataSources(DataSource dataSource)
        {
            _applicationContext.DataSources.Update(dataSource);
            _applicationContext.SaveChanges();
            _notifyService.DataBaseDataUpdated();
        }
        #endregion

        public OperationType GetOperationTypeBySumm(decimal summ)
        {
            if(summ < 0)
            {
                return _applicationContext.OperationTypes.First(x => x.Id == 1);
            }
            else
            {
                return _applicationContext.OperationTypes.First(x => x.Id == 2);
            }
        }

    }
}
