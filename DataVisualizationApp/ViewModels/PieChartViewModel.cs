using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


// Create PieSeries by School Type (Public/Private)


namespace DataVisualizationApp.ViewModels
{
public class PieChartViewModel : INotifyPropertyChanged
{
    private IEnumerable<ISeries> _series = new List<ISeries>();
    public IEnumerable<ISeries> Series
    {
        get => _series;
        set { _series = value; OnPropertyChanged(); }
    }

    public PieChartViewModel()
{
        LoadDataFromCSV();
}

    private void LoadDataFromCSV()
    {
        List<StudentPerformance> students = CsvService.LoadCsv();

            // Use the query from MainWindowViewModel for total attendance by school type
        var schoolCounts = students
                .GroupBy(s => s.School_Type) 
                .Select(g => new
                {
                    SchoolType = g.Key,
                    TotalAttendance = g.Sum(s => s.Attendance) // You can sum attendance here or adapt to your needs
                })
                .ToList();

        Series = schoolCounts.Select(kvp =>
                new PieSeries<int> { Values = new int[] { kvp.TotalAttendance }, Name = kvp.SchoolType }).ToList();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
}