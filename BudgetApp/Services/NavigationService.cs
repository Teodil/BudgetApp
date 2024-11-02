using BudgetApp.Models.Core;
using BudgetApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BudgetApp.Services
{
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }

        Window GetWindow<TWindow>() where TWindow : Window;
        void NavigateTo<T>() where T : ViewModelBase;
    }

    public class NavigationService : ObservableObject, INavigationService
    {

        private ViewModelBase _currentView;
        private readonly Func<Type, ViewModelBase> _viewModelFactory;
        private readonly Func<Type, Window> _windowFactory;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            private set
            {
                Set(ref _currentView, value);
            }
        }

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory, Func<Type,Window> windowFactory)
        {
            _viewModelFactory = viewModelFactory;
            _windowFactory = windowFactory;
        }

        public void NavigateTo<TViewModelBase>() where TViewModelBase : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModelBase));
            CurrentView = viewModel;
        }

        public Window GetWindow<TWindow>() where TWindow : Window
        {
            return _windowFactory.Invoke(typeof(TWindow));
        }
    }
}
