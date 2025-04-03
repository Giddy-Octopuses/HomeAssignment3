using System;
using System.Collections.Generic;
namespace DataVisualizationApp.ViewModels;

using System.ComponentModel;
using System.Globalization;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataVisualizationApp.Models;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly List<StudentPerformance> _data;

    public MainWindowViewModel()
    {

        _data = CsvService.LoadCsv();

        Console.WriteLine($"Loaded {_data.Count} records from CSV.");
    }

    [ObservableProperty] private string? selectedQuery;
    [ObservableProperty] private bool popupOpen;
    [ObservableProperty] private string message = string.Empty;
    [ObservableProperty] private string colour = string.Empty;
    [ObservableProperty] private bool deleteButtonVisible = true; // THIS SHOULD BE CHANGED! -- initial: false; set to true when you click on a chart

    [RelayCommand]
    public async Task AddChart()
    {
        if (string.IsNullOrEmpty(SelectedQuery))
        {
            await ShowPopup("Error: Please choose a query before clicking 'Add chart'.", "Red");
            return;
        }
        else
        {
            Console.WriteLine("Add Chart command executed.");
        }
    }

    [RelayCommand]
    public void DeleteChart()
    {
        Console.WriteLine("Delete Chart command executed.");
    }

    private async Task ShowPopup(string message, string colour, int duration = 3000)
    {
        Message = message;
        Colour = colour;
        PopupOpen = true;
        await Task.Delay(duration);
        PopupOpen = false;
    }
}
