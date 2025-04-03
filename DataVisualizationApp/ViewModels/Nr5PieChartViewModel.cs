using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System;


//Students grades based on their sleep hours


namespace DataVisualizationApp.ViewModels
{
    public class Nr5PieChartViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ISeries> _series = new List<ISeries>();
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set { _series = value; OnPropertyChanged(); }
        }

        public Nr5PieChartViewModel()
        {
            LoadDataFromCSV();
        }

        private void LoadDataFromCSV()
        {
            List<StudentPerformance> students = CsvService.LoadCsv();

            // Group students by sleep ranges (4-5, 5-6, etc.)
            var sleepRanges = students
                .Where(d => d.Sleep_Hours >= 4 && d.Sleep_Hours < 11) // Filter students with 4-10 hours of sleep
                .GroupBy(d => (int)Math.Floor((double)d.Sleep_Hours)) 
                .ToDictionary(g => $"{g.Key}-{g.Key + 1} hours", g => g.Count()); 

            Series = sleepRanges.Select(kvp =>
                new PieSeries<int>
                {
                    Values = new int[] { kvp.Value },
                    Name = $"{kvp.Key}",
                }).ToList();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
