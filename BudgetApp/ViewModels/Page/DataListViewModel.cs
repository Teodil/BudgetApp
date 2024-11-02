using BudgetApp.Infrastructure.Context;
using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.Services;
using BudgetApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BudgetApp.ViewModels.Page
{
    public class DataListViewModel : ViewModelBase
    {
        private List<CardOperation> cardOperations = new List<CardOperation>();
        public List<CardOperation> CardOperations
        {
            get { return cardOperations; }
            set { Set(ref cardOperations, value); }
        }

        private NotifyService _notifyService;
        private Repository _repository;

        public DataListViewModel(NotifyService notifyService, Repository repository)
        {
            _notifyService = notifyService;
            _repository = repository;
            _notifyService.DataParsedEvent += RefreshData;
        }

        private void RefreshData()
        {
            CardOperations = _repository.GetCardOperations(); ;
        }


    }
}
