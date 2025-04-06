using LiveChartsCore;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataVisualizationApp.ViewModels
{
    public abstract class ChartViewModelBase : ViewModelBase, INotifyPropertyChanged
    {
        private IEnumerable<ISeries> _series = new List<ISeries>();

        public IEnumerable<ISeries> Series
        {
            get => _series;
            set
            {
                if (_series != value)
                {
                    _series = value;
                    OnPropertyChanged();
                }
            }
        }

        // Abstract method for loading data
        protected abstract void LoadData();

        public ChartViewModelBase()
        {
        }
    }
}
