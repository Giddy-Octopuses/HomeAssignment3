using System;
using System.Collections.Generic;
namespace DataVisualizationApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    private readonly List<StudentPerformance> _data;

    public MainWindowViewModel()
    {
        
        _data = CsvService.LoadCsv();

        Console.WriteLine($"Loaded {_data.Count} records from CSV.");
    }
}
