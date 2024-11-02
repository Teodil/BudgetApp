using BudgetApp.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.Models.DTO
{
    public class ParserResultDTO
    {
        public List<CardOperation> newCardOperations;
        public List<OperationCategory> newOperationCategories;

        public ParserResultDTO(List<CardOperation> newCardOperations, List<OperationCategory> newOperationCategories)
        {
            this.newCardOperations = newCardOperations;
            this.newOperationCategories = newOperationCategories;
        }
    }
}
