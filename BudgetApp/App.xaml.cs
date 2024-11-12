using BudgetApp.Infrastructure.Context;
using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.Models.Utilitis;
using BudgetApp.Services;
using BudgetApp.ViewModels.Base;
using BudgetApp.ViewModels.Page;
using BudgetApp.ViewModels.Window;
using BudgetApp.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows;

namespace BudgetApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddScoped<Parser>();

            Type serviceType = typeof(CardOperation);

            foreach(PropertyInfo prop in serviceType.GetProperties())
            {
                //var attrs = prop.GetCustomAttributes(true);
                var paramNameAttribute = prop.GetCustomAttribute(typeof(PoleDescriptionAttribute)) as PoleDescriptionAttribute;
                
                string paramNameValue = paramNameAttribute is null ? prop.Name : paramNameAttribute.Name;
                string propName = prop.Name;
       
            }

            services.AddSingleton<EventTransferService>();
            services.AddSingleton<TransferObjectService>();

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoadDataWindowViewModel>();
            services.AddTransient<DataListViewModel>();
            services.AddTransient<DataSourceConrolWindowViewModel>();

            services.AddSingleton<MainWindow>(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            });
            services.AddTransient<LoadDataWindow>(provider => new LoadDataWindow()
            {
                DataContext = provider.GetRequiredService<LoadDataWindowViewModel>()
            });
            services.AddTransient<DataSourceConrolWindow>(provider => new DataSourceConrolWindow()
            {
                DataContext = provider.GetRequiredService<DataSourceConrolWindowViewModel>()
            });

            services.AddDbContext<ApplicationContext>(opt =>
                    opt.UseSqlite("Data Source=AppDatabase.db"));
            services.AddSingleton<Repository>();
            
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModelBase>>(servicesProvider => viewModelType => (ViewModelBase)servicesProvider.GetRequiredService(viewModelType));
            services.AddSingleton<Func<Type, Window>>(servicesProvider => windowType => (Window)servicesProvider.GetRequiredService(windowType));


            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            mainWindow.Closed += CloseApp;
            base.OnStartup(e);
        }

        private void CloseApp(object? sender, EventArgs e)
        {
            if (sender != this)
                this.Shutdown();
        }

    }

}
