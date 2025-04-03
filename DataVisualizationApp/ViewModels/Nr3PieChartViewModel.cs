using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


// Create PieSeries for students with Postgraduate parental education level and Group students by their exam score ranges


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

            // Filter out students with Postgraduate parental education level
            var postgraduateStudents = students.Where(s => s.Parental_Education_Level == "Postgraduate").ToList();
            var totalStudents = postgraduateStudents.Count;

            // Define the score ranges
            var scoreRanges = new Dictionary<string, int>
            {
                { "50-60", 0 },
                { "61-70", 0 },
                { "71-80", 0 },
                { "81-90", 0 },
                { "91-100", 0 }
            };

            // Group students by their exam score ranges
            foreach (var student in postgraduateStudents)
            {
                string scoreRange = GetScoreRange(student.Exam_Score);
                if (scoreRanges.ContainsKey(scoreRange))
                {
                    scoreRanges[scoreRange]++;
                }
            }

            // Create PieSeries for each score range
            Series = scoreRanges.Select(kvp =>
                new PieSeries<int>
                {
                    Values = [kvp.Value],
                    Name = $"{kvp.Key}",
                    DataLabelsFormatter = point => $"{point.Context.DataSource}" // Display the number of students
                }).ToList();
        }

        // Method to determine the score range based on the exam score
        private string GetScoreRange(double score)
        {
            if (score <= 60) return "50-60";
            if (score <= 70) return "61-70";
            if (score <= 80) return "71-80";
            if (score <= 90) return "81-90";
            return "91-100";
        }

        // Implement the PropertyChanged event
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
