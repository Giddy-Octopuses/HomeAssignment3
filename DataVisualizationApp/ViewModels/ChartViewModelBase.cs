using LiveChartsCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChartViewModelBase()
        {
        }
    }
}
