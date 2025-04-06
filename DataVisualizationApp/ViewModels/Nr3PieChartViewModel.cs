using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


//Students with Exam Score [a pie chart with exam scores 0-10; 11-20; 21-30 and so on]


namespace DataVisualizationApp.ViewModels
{
    public class Nr3PieChartViewModel : ChartViewModelBase
    {
        public Nr3PieChartViewModel()
        {
            LoadData();
        }
        protected override void LoadData()
        {
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
            Series = allStudents.Select(kvp =>
                new PieSeries<int>
                {
                    Values = new int[] { kvp.Count },
                    Name = $"{kvp.ScoreRange}",
                    DataLabelsFormatter = point => $"{point.Context.DataSource}" // Display the number of students
                }).ToList();
        }
    }
}
