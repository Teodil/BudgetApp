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

        private EventTransferService _eventTransferService;
        private Repository _repository;

        public DataListViewModel(EventTransferService eventTransferService, Repository repository)
        {
            _eventTransferService = eventTransferService;
            _repository = repository;
            _eventTransferService.DataBaseDataUpdatedEvent += RefreshData;
        }

        private void RefreshData()
        {
            CardOperations = _repository.GetCardOperations(); ;
        }


    }
}
