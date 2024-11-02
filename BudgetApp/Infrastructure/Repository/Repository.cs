using BudgetApp.Infrastructure.Context;
using BudgetApp.Models.Data;
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

        public Repository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #region CardOperation
        public List<CardOperation> GetCardOperations(DateTime? from = null,DateTime? to = null, OperationCategory? operationCategory = null, OperationType? operationType = null)
        {
            var query = _applicationContext.CardOperations.AsQueryable();
            if (from != null)
                query = query.Where(x => x.Date >= from);
            if (to != null)
                query = query.Where(x => x.Date <= to);
            if (operationCategory != null)
                query = query.Where(x => x.OperationCategory == operationCategory);
            if (operationType != null)
                query = query.Where(x => x.OperationType == operationType);

            return query.ToList();
        }

        public bool IsCardOperationExist(CardOperation operation)
        {
            return _applicationContext.CardOperations.Any(x=>x.Date == operation.Date && x.Bank == operation.Bank && x.Summ == operation.Summ);
        }

        public void AddCardOperation(CardOperation operation)
        {
            _applicationContext.CardOperations.Add(operation);
            _applicationContext.SaveChanges();
        }

        public void AddRangeCardOperation(IEnumerable<CardOperation> operations)
        {
            _applicationContext.CardOperations.AddRange(operations);
            _applicationContext.SaveChanges();
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
        }

        public void AddRangeOperationCategory(IEnumerable<OperationCategory> categories)
        {
            _applicationContext.OperationCategories.AddRange(categories);
            _applicationContext.SaveChanges();
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

        public Bank? GetBank(int id)
        {
            return _applicationContext.Banks.FirstOrDefault(x => x.Id == id);
        }

        public List<Bank> GetBanks()
        {
            return _applicationContext.Banks.ToList();
        }

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
