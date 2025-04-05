using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


//Number of Students by Peer Influence  {pie chart}


namespace DataVisualizationApp.ViewModels
{
    public class Nr2PieChartViewModel : ViewModelBase // INotifyPropertyChanged
    {
        private IEnumerable<ISeries> _series = new List<ISeries>(); 
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set { _series = value; OnPropertyChanged(); }
        }

        public Nr2PieChartViewModel()
        {
            LoadDataFromCSV();
        }

        private void LoadDataFromCSV()
        {
            List<StudentPerformance> students = CsvService.LoadCsv();
            var peerInfluenceCounts = students
                .GroupBy(s => s.Peer_Influence) 
                .ToDictionary(g => g.Key, g => g.Count());
            Series = peerInfluenceCounts.Select(kvp =>
                new PieSeries<int>
                {
                    Values = [kvp.Value],
                    Name = $"{kvp.Key}" 
                }).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}