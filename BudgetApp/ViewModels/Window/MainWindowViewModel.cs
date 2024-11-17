using BudgetApp.Infrastructure.Command;
using BudgetApp.Services;
using BudgetApp.ViewModels.Base;
using BudgetApp.ViewModels.Page;
using BudgetApp.Views.Pages;
using BudgetApp.Views.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetApp.ViewModels.Window
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get { return _navigationService; }
        }

        private Parser _parser;

        private LoadDataWindow _loadDataWindow;

        public MainWindowViewModel(INavigationService navigationService, Parser parser, LoadDataWindow loadDataWindow)
        {
            _navigationService = navigationService;
            _navigationService.NavigateTo<DataListViewModel>();
            _parser = parser;
            _loadDataWindow = loadDataWindow;
            OnLoadCommand = new RelayCommand(Load);
            AnaliticCommand = new RelayCommand(OnAnaliticCommand);
        }


        public ICommand OnLoadCommand { get; set; }

        private void Load(object param)
        {
            _navigationService.GetWindow<LoadDataWindow>().Show();

        }

        public ICommand AnaliticCommand { get; set; }

        private void OnAnaliticCommand(object param)
        {
            _navigationService.NavigateTo<AnaliticPageViewModel>();

        }


    }
}
