using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.Linq;
using System;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;


//Students based on their sleep hours


namespace DataVisualizationApp.ViewModels
{
    public class Nr5PieChartViewModel : ChartViewModelBase
    {
        public Nr5PieChartViewModel()
        {
            LoadData();
        }
        protected override void LoadData()
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
                    Stroke = new SolidColorPaint(SKColors.Black),
                }).ToList();
        }
    }
}
