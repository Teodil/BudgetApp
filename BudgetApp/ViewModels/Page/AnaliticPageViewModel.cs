using BudgetApp.Infrastructure.Repository;
using BudgetApp.Models.Data;
using BudgetApp.ViewModels.Base;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApp.ViewModels.Page
{
    public class AnaliticPageViewModel : ViewModelBase
    {
        private Repository _repository;

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                Set(ref _fromDate, value);
                LoadCharts();
            }
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get=> _toDate;
            set {
                Set(ref _toDate, value);
                LoadCharts();
            }
        }

        private SeriesCollection _categoryCollection = new SeriesCollection();
        public SeriesCollection CategoryCollection { get=>_categoryCollection; set=>Set(ref _categoryCollection,value); }

        private SeriesCollection _dayCollection = new SeriesCollection();
        public SeriesCollection DayCollection { get => _dayCollection; set => Set(ref _dayCollection, value); }

        private List<string> _labels;
        public List<string> Labels { get=>_labels; set=>Set(ref _labels,value); }
        private Func<double, string> _formatter;
        public Func<double, string> Formatter { get=>_formatter; set=>Set(ref _formatter,value); }

        public AnaliticPageViewModel(Repository repository)
        {
            _repository = repository;

            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = DateTime.Now;
            LoadCharts();
            Formatter = value => value.ToString("N");

        }

        private void LoadCharts()
        {
            List<CardOperation> operations = _repository.GetCardOperations(FromDate, ToDate);

            var categoryGroup = operations.GroupBy(x => x.OperationCategory).Select(x => new { Name = x.Key.Name, Summ = x.Sum(t => t.Summ) });

            SeriesCollection newColection = new SeriesCollection();
            foreach (var operation in categoryGroup)
            {
                newColection.Add(new PieSeries { Title = operation.Name, Values = new ChartValues<ObservableValue> { new ObservableValue((double)operation.Summ) }, DataLabels = true });
            }
            CategoryCollection = newColection;

            var categoryGroup2 = operations.GroupBy(x => x.OperationCategory).Select(x=>x).ToList();

            newColection = new SeriesCollection();
            List<string> lables = new List<string>();
            int days = (ToDate - FromDate).Days+1;
            foreach (var operation in categoryGroup2)
            {
                List<double> values = new List<double>();
                for (int i=0;i< days; i++)
                {
                    DateTime dateTime = FromDate.AddDays(i);
                    var operationsInDay = operation.Where(x => x.Date.Date == dateTime.Date).ToList();
                    if (operationsInDay.Count > 0)
                        values.Add(operationsInDay.Sum(x => (double)x.Summ));
                    else 
                        values.Add(0);
                }
                newColection.Add(new StackedColumnSeries
                {
                    Values = new ChartValues<double>(values),
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true,
                    Title = operation.Key.Name
                });
            }

            for (int i = 0; i < days; i++)
            {
                DateTime dateTime = FromDate.AddDays(i);
                lables.Add(dateTime.ToString("dd-MM-yyyy"));
            }

            Labels = lables;
            DayCollection = newColection;

        }
    }
    
}
