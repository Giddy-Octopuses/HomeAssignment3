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

            // Group by Peer Influence and calculate percentage
            var peerInfluenceCounts = students
                .GroupBy(s => s.Peer_Influence) // Group by Peer Influence
                .ToDictionary(g => g.Key, g => g.Count());

            var totalStudents = students.Count();

            // Create PieSeries for each PeerInfluence group with percentage
            Series = peerInfluenceCounts.Select(kvp =>
                new PieSeries<int>
                {
                    Values = new int[] { (int)((double)kvp.Value / totalStudents * 100) },
                    Name = kvp.Key
                }).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}