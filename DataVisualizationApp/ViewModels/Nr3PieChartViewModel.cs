using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DataVisualizationApp.ViewModels
{
    public class Nr3PieChartViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ISeries> _series = new List<ISeries>(); // Initialize with an empty collection
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set { _series = value; OnPropertyChanged(); }
        }

        public Nr3PieChartViewModel()
        {
            LoadDataFromCSV();
        }

        private void LoadDataFromCSV()
        {
            // Load the student data
            List<StudentPerformance> students = CsvService.LoadCsv();

            // If needed, filter by parental education level
            // var postgraduateParentalEducation = students
            //     .Where(s => s.Parental_Education_Level == "Postgraduate");

            // Include all students regardless of parental education level
            var allStudents = students
                .GroupBy(s => s.Exam_Score / 10)
                .Select(g => new
                {
                    ScoreRange = g.Key * 10 + "-" + ((g.Key + 1) * 10 - 1),
                    Count = g.Count()
                })
                .ToList();

            // Create PieSeries for each score range from the query results
            Series = allStudents.Select(kvp =>
                new PieSeries<int>
                {
                    Values = new int[] { kvp.Count },
                    Name = $"{kvp.ScoreRange}",
                    DataLabelsFormatter = point => $"{point.Context.DataSource}" // Display the number of students
                }).ToList();
        }

        // Implement the PropertyChanged event
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
