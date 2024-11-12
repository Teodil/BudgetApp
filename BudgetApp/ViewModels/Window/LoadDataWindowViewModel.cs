using BudgetApp.Infrastructure.Command;
using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.Models.DTO;
using BudgetApp.Services;
using BudgetApp.ViewModels.Base;
using BudgetApp.Views.Windows;
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
        private readonly Parser _parser;
        private readonly Repository _repository;
        private readonly INavigationService _navigationService;
        private readonly TransferObjectService _transferObjectService;
        private readonly EventTransferService _eventTransferService;

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

        private ObservableCollection<OperationCategory> _newOperationCategories = new ObservableCollection<OperationCategory>();
        public ObservableCollection<OperationCategory> NewOperationCategories
        {
            get=>_newOperationCategories;
            set=>Set(ref _newOperationCategories, value);
        }

        private ObservableCollection<OperationCategory> _operationCategories;
        public ObservableCollection<OperationCategory> OperationCategories
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

        private bool _isLoadDuplicate = false;
        public bool IsLoadDuplicate
        {
            get=>_isLoadDuplicate;
            set=> Set(ref _isLoadDuplicate, value);
        }

        public LoadDataWindowViewModel(Parser parser, Repository repository, 
                                       INavigationService navigationService, 
                                       TransferObjectService transferObjectService,
                                       EventTransferService eventTransferService)
        {
            _parser = parser;
            _repository = repository;
            LoadData();
            FilePath = "";
            OpenFileCommand = new RelayCommand(OnOpenFileCommand);
            LoadCommand = new RelayCommand(OnLoadCommand, CanLoadCommand);
            ClearCommand = new RelayCommand(OnClearCommand, CanClearCommand);
            EditDataSourceCommand = new RelayCommand(OnEditDataSourceCommand, CanEditDataSourceCommand);
            AddDataSourceCommand = new RelayCommand(OnAddDataSourceCommand);
            _navigationService = navigationService;
            _transferObjectService = transferObjectService;
            _eventTransferService = eventTransferService;
            eventTransferService.DataBaseDataUpdatedEvent += LoadData;
        }

        private void LoadData()
        {
            OperationCategories = new ObservableCollection<OperationCategory>(_repository.GetOperationCategories());
            DataSources = _repository.GetDataSources();
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
                foreach (var item in UploadData)
                {
                    OperationCategory operationCategory = new OperationCategory { Name = item.OperationCategory };
                    if (NewOperationCategories.FirstOrDefault(x => x.Name == item.OperationCategory) == null && _repository.IsOperationCategoryExist(operationCategory) == false)
                    {
                        NewOperationCategories.Add(operationCategory);
                    }
                }


            }
        }

        public ICommand EditDataSourceCommand { get; set; }
        private void OnEditDataSourceCommand(object param)
        {
            _transferObjectService.SetTransferObject(CurrentDataSource);
            _navigationService.GetWindow<DataSourceConrolWindow>().Show();
        }

        private bool CanEditDataSourceCommand(object param)
        {
            if(CurrentDataSource != null) return true;

            return false;
        }

        public ICommand AddDataSourceCommand { get; set; }
        private void OnAddDataSourceCommand(object param)
        {
            _navigationService.GetWindow<DataSourceConrolWindow>().Show();
        }

        public ICommand LoadCommand { get; set; }
        private void OnLoadCommand(object param)
        {
            List<UploadDataDTO> dataToUpload = UploadData.ToList();
            if(IsLoadDuplicate == false)
                dataToUpload = UploadData.Where(x=>x.IsDuplicate == false).ToList();

            foreach (var item in dataToUpload)
            {
                OperationCategory operationCategory = new OperationCategory { Name = item.OperationCategory };

                operationCategory = NewOperationCategories.FirstOrDefault(x => x.Name == item.OperationCategory) ?? _repository.GetOperationCategoryByName(item.OperationCategory);

                CardOperation cardOperation = new CardOperation(item);
                cardOperation.OperationCategory = operationCategory;
                
                _repository.AddCardOperation(cardOperation);
            }

            UploadData = null;
            NewOperationCategories.Clear();
            LoadData();
            MessageBox.Show("Данные загружены и сохранены");
        }

        private bool CanLoadCommand(object param)
        {
            if (UploadData == null  || UploadData.Count == 0) return false;

            return true;
        }

        public ICommand ClearCommand { get; set; }
        private void OnClearCommand(object param)
        {
            UploadData = null;
            NewOperationCategories.Clear();
            MessageBox.Show("Данные удалены");
        }

        private bool CanClearCommand(object param)
        {
            if (UploadData == null)
                return false;
            return true;
        }

        #endregion
    }
}
