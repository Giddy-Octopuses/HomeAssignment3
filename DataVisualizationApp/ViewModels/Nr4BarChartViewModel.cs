using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.Generic;
using System.Linq;
using System;



//Average Exam Score based on Physical Activity {bar chart}


namespace DataVisualizationApp.ViewModels
{
    public class Nr4BarChartViewModel : ChartViewModelBase
    {
        private List<string> _labels = new List<string>();
        public List<string> Labels
        {
            get => _labels;
            set { _labels = value; OnPropertyChanged(); }
        }

        private readonly List<StudentPerformance> _data;

        public Nr4BarChartViewModel()
        {
            _data = CsvService.LoadCsv();
            LoadData();
        }

        protected override void LoadData()
        {
            var avgScoreByPhysicalActivity = _data
                .GroupBy(d => d.Physical_Activity)
                .Select(g => new
                {
                    PhysicalActivity = g.Key,
                    AverageScore = Math.Round(g.Average(d => d.Exam_Score), 1)
                })
                .ToList();

            Labels = avgScoreByPhysicalActivity.Select(g => g.PhysicalActivity.ToString()).ToList();

            Series = new List<ISeries>
            {
                new ColumnSeries<double>
                {
                    Values = avgScoreByPhysicalActivity.Select(g => g.AverageScore).ToList(),
                    Name = "Average Exam Score",
                    MaxBarWidth = 50,
                    Fill = new SolidColorPaint(SKColors.Yellow),

                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                    Stroke = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 16,
                    DataLabelsFormatter = point => point.Model.ToString("0.0")
                }
            };
        }
    }
}
