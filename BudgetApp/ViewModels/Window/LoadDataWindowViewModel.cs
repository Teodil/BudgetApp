using BudgetApp.Infrastructure.Command;
using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.Models.DTO;
using BudgetApp.Services;
using BudgetApp.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetApp.ViewModels.Window
{
    public class LoadDataWindowViewModel : ViewModelBase
    {
        private Parser _parser;
        private Repository _repository;

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set => Set(ref _filePath, value);
        }

        private List<CardOperation> _cardOperations;
        public List<CardOperation> CardOperations
        {
            get=> _cardOperations;
            set=> Set(ref _cardOperations, value);
        }

        private List<OperationCategory> _newOperationCategories;
        public List<OperationCategory> NewOperationCategories
        {
            get=>_newOperationCategories;
            set=>Set(ref _newOperationCategories, value);
        }

        private List<OperationCategory> _operationCategories;
        public List<OperationCategory> OperationCategories
        {
            get=> _operationCategories;
            set => Set(ref _operationCategories, value);
        }


        private List<Bank> _banks;
        public List<Bank> Banks
        {
            get=>_banks;
            set=>Set(ref _banks, value);
        }

        public LoadDataWindowViewModel(Parser parser, Repository repository)
        {
            _parser = parser;
            _repository = repository;
            OperationCategories = repository.GetOperationCategories();
            Banks = repository.GetBanks();
            FilePath = "test";
            OpenFileCommand = new RelayCommand(OnOpenFileCommand);
        }


        public ICommand OpenFileCommand
        {
            get; set;
        }
        private void OnOpenFileCommand(object param)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*";
            if(openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                ParserResultDTO parserResult = _parser.Parse(openFileDialog.FileName, null);
                CardOperations = parserResult.newCardOperations;
                NewOperationCategories = parserResult.newOperationCategories;
            }
        }
    }
}
