using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

//Percentage of Students by School Type  {pie chart}

namespace DataVisualizationApp.ViewModels;

public class PieChartViewModel : ChartViewModelBase
{
    protected override void LoadData()
    {
        List<StudentPerformance> students = CsvService.LoadCsv();

        var schoolCounts = students
            .GroupBy(s => s.School_Type) // Group by School Type (Public/Private)
            .ToDictionary(g => g.Key, g => g.Count());

        Series = schoolCounts.Select(kvp =>
            new PieSeries<int> { Values = [kvp.Value], Name = kvp.Key }).ToList();
    }
}
