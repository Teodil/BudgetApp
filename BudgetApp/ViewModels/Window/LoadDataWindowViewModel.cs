using BudgetApp.Infrastructure.Command;
using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.Models.DTO;
using BudgetApp.Services;
using BudgetApp.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private ObservableCollection<UploadDataDTO> _uploadData;
        public ObservableCollection<UploadDataDTO> UploadData
        {
            get => _uploadData;
            set => Set(ref _uploadData, value);
        }


        private List<DataSource> _dataSources;
        public List<DataSource> DataSources
        {
            get=>_dataSources;
            set=>Set(ref _dataSources, value);
        }

        private DataSource _currentDataSource;
        public DataSource CurrentDataSource
        {
            get=>_currentDataSource;
            set => Set(ref _currentDataSource, value);
        }

        public LoadDataWindowViewModel(Parser parser, Repository repository)
        {
            _parser = parser;
            _repository = repository;
            OperationCategories = repository.GetOperationCategories();
            DataSources = repository.GetBanks();
            FilePath = "";
            OpenFileCommand = new RelayCommand(OnOpenFileCommand);
            EditBankMapCommand = new RelayCommand(OnEditBankMapCommand);
        }

        #region Commands
        public ICommand OpenFileCommand
        {
            get; set;
        }
        private void OnOpenFileCommand(object param)
        {
            if(CurrentDataSource == null)
            {
                MessageBox.Show("Не выбран шаблон загрузки данных");
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*";
            if(openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                List<UploadDataDTO> parserResult = _parser.Parse(openFileDialog.FileName, CurrentDataSource);
                UploadData = new ObservableCollection<UploadDataDTO>(parserResult);
                //CardOperations = parserResult.newCardOperations;
                //NewOperationCategories = parserResult.newOperationCategories;
            }
        }

        public ICommand EditBankMapCommand
        {
            get;
            set;
        }

        private void OnEditBankMapCommand(object param)
        {
            Console.WriteLine(param);
        }

        #endregion
    }
}
