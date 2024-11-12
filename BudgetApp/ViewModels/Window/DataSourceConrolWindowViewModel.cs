using BudgetApp.Infrastructure.Command;
using BudgetApp.Infrastructure.Context;
using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.Models.DTO;
using BudgetApp.Models.Utilitis;
using BudgetApp.Services;
using BudgetApp.ViewModels.Base;
using LogicExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BudgetApp.ViewModels.Window
{
    public class DataSourceConrolWindowViewModel : ViewModelBase
    {
        private readonly Repository _repository;
        private readonly TransferObjectService _transferObjectService;

        private DataSource _dataSource;
        public DataSource DataSource { get=>_dataSource; set=>Set(ref(_dataSource),value); }

        private string _dataSourceName;
        public string DataSourceName
        {
            get => _dataSourceName;
            set => Set(ref _dataSourceName, value);
        }

        private ObservableCollection<PoleAccordanceInputDTO> _poleAccordances = new ObservableCollection<PoleAccordanceInputDTO>();

        public ObservableCollection<PoleAccordanceInputDTO> PoleAccordances
        {
            get=> _poleAccordances;
            set=>Set(ref  _poleAccordances, value);
        }


        public DataSourceConrolWindowViewModel(Repository repository, TransferObjectService transferObjectService)
        {
            _repository = repository;
            _transferObjectService = transferObjectService;
            DataSource? dataSource = _transferObjectService.GetTransferObjectAndDelete<DataSource>();
            if(dataSource != null)
            {
                DataSource = dataSource;
                DataSourceName = dataSource.Name;
            }
            Type type = typeof(CardOperation);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                PoleAccordanceInputDTO poleAccordance = new PoleAccordanceInputDTO();
                var paramNameAttribute = property.GetCustomAttribute(typeof(PoleDescriptionAttribute)) as PoleDescriptionAttribute;
                if (paramNameAttribute?.IsEditable == false || property.Name == nameof(DataSource))
                    continue;
                string paramNameValue = paramNameAttribute is null ? property.Name : paramNameAttribute.Name;
                poleAccordance.PoleTitle = paramNameValue;
                poleAccordance.PoleName = property.Name;
                poleAccordance.IsRequired = paramNameAttribute?.IsRequired ?? true;
                if(dataSource != null)
                {
                    string excelColumnName = dataSource.PoleAccordances?.FirstOrDefault(x => x.PoleName == poleAccordance.PoleName)?.ExcelColumnName ?? null;
                    poleAccordance.ExcelColumnName = excelColumnName;
                }
                _poleAccordances.Add(poleAccordance);
            }

            SaveCommand = new RelayCommand(OnSaveCommand, CanSaveCommand);
        }

        public ICommand SaveCommand { get; set; }

        public void OnSaveCommand (object param)
        {
            bool isNewObject = false;
            if(DataSource == null)
            {
                DataSource = new DataSource();
                isNewObject = true;
            }
            DataSource.Name = DataSourceName;
            DataSource.PoleAccordances = PoleAccordances.Select(x=>x.CreatePoleAccordance()).ToList();

            if (isNewObject)
                _repository.AddDataDataSources(DataSource);
            else
                _repository.UpdateDataDataSources(DataSource);

            MessageBox.Show("Данные успешно сохранены");
        }

        public bool CanSaveCommand(object param)
        {
            if (PoleAccordances.Any(x => x.IsRequired == true && x.ExcelColumnName.IsNullOrEmpty()) == true)
                return false;

            return true;
        }
    }
}
