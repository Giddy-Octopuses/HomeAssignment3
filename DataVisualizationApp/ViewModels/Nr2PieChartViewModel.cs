using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DataVisualizationApp.ViewModels
{
    public class Nr2PieChartViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ISeries> _series = new List<ISeries>(); // Initialize with an empty collection
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

            // Group by Peer Influence and count the number of students in each group
            var peerInfluenceCounts = students
                .GroupBy(s => s.Peer_Influence) // Group by Peer Influence
                .ToDictionary(g => g.Key, g => g.Count());

            // Create PieSeries for each Peer Influence group with the count of students
            Series = peerInfluenceCounts.Select(kvp =>
                new PieSeries<int>
                {
                    Values = new int[] { kvp.Value }, // Number of students in this Peer Influence group
                    Name = $"{kvp.Key}" // Label showing the group and count
                }).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}