using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using System.Runtime.CompilerServices;
using LiveChartsCore.SkiaSharpView.Painting;

// Motivation Level by gender {Dohnut chart}


namespace DataVisualizationApp.ViewModels
{
    public class Nr6PieChartViewModel : ViewModelBase
    {
        private IEnumerable<ISeries> _series = new List<ISeries>();
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set { _series = value; OnPropertyChanged(); }
        }

        private List<string> _labels = new List<string>();
        public List<string> Labels
        {
            get => _labels;
            set { _labels = value; OnPropertyChanged(); }
        }

        private readonly List<StudentPerformance> _data;

        public Nr6PieChartViewModel()
        {
            _data = CsvService.LoadCsv();
            LoadDataFromCSV();
        }

        private void LoadDataFromCSV()
        {
            // Define a mapping for Motivation_Level
            var motivationMapping = new Dictionary<string, int>
            {
                { "Low", 1 },
                { "Medium", 2 },
                { "High", 3 }
            };

            var avgMotivationByGender = _data
                .Where(d => motivationMapping.ContainsKey(d.Motivation_Level)) 
                .GroupBy(d => d.Gender)
                .Select(g => new
                {
                    Gender = g.Key,
                    AverageMotivation = Math.Round(g.Average(d => motivationMapping[d.Motivation_Level]), 3) 
                })
                .ToList();

            Labels = avgMotivationByGender.Select(g => g.Gender).ToList();

            Series = new List<ISeries>
            {
                // Inner part ( Male)
                new PieSeries<double>
                {
                    Values = [avgMotivationByGender.FirstOrDefault(g => g.Gender == "Male")?.AverageMotivation ?? 0],
                    Name = "Male",
                    Fill = new SolidColorPaint(SKColors.LightBlue),
                    Stroke = new SolidColorPaint(SKColors.DarkBlue),
                    InnerRadius = 0
                },
                // Outer part ( Female)
                new PieSeries<double>
                {
                    Values = [avgMotivationByGender.FirstOrDefault(g => g.Gender == "Female")?.AverageMotivation ?? 0],
                    Name = "Female",
                    Fill = new SolidColorPaint(SKColors.Pink),
                    Stroke = new SolidColorPaint(SKColors.White),
                    InnerRadius = 40
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
