using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataVisualizationApp.Views;

namespace DataVisualizationApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly List<StudentPerformance> _data;

        public MainWindowViewModel()
        {

            _data = CsvService.LoadCsv();
            Console.WriteLine($"Loaded {_data.Count} records from CSV.");

            // Queries available for selection
            Queries = new ObservableCollection<string>
        {
            "Number of Students by School Type",
            "Number of Students by Peer Influence",
            "Average Exam Score of Students",
            "Average Exam Score by Physical Activity",
            "Average Sleep Hours of Students",
            "Motivation Level by Gender"
        };

            // Initialize chart slots (6 slots, all empty initially)
            ChartSlots = new ObservableCollection<ViewModelBase?>(new ViewModelBase?[6]);
            SelectedChartIndex = -1; // No chart is selected initially
        }

        // Stacks for undo and redo actions
        private readonly Stack<List<ViewModelBase?>> UndoStack = new Stack<List<ViewModelBase?>>();
        private readonly Stack<List<ViewModelBase?>> RedoStack = new Stack<List<ViewModelBase?>>();


        public ObservableCollection<string> Queries { get; }
        public ObservableCollection<ViewModelBase?> ChartSlots { get; }
        [ObservableProperty] private string? selectedQuery;
        [ObservableProperty] private ViewModelBase? selectedChartViewModel;
        [ObservableProperty] private int selectedChartIndex; // Tracks the selected chart slot index
        [ObservableProperty] private bool popupOpen;
        [ObservableProperty] private string message = string.Empty;
        [ObservableProperty] private string colour = string.Empty;
        [ObservableProperty] private bool deleteButtonVisible = false; // Initially hidden; appears when you click on a chart

        // Undo/Redo Commands
        [RelayCommand]
        public void Undo()
        {
            if (UndoStack.Count == 0)
            {
                _ = ShowPopup("No actions to undo.", "Red");
                return;
            }

            var previousState = UndoStack.Pop();
            RedoStack.Push(new List<ViewModelBase?>(ChartSlots)); // Store the current state in redo stack
            ChartSlots.Clear();
            foreach (var chart in previousState)
            {
                ChartSlots.Add(chart);
            }

            Console.WriteLine("Undo executed.");
        }

        [RelayCommand]
        public void Redo()
        {
            if (RedoStack.Count == 0)
            {
                _ = ShowPopup("No actions to redo.", "Red");
                return;
            }

            var nextState = RedoStack.Pop();
            UndoStack.Push(new List<ViewModelBase?>(ChartSlots)); // Store the current state in undo stack
            ChartSlots.Clear();
            foreach (var chart in nextState)
            {
                ChartSlots.Add(chart);
            }

            Console.WriteLine("Redo executed.");
        }

        [RelayCommand]
        public async Task AddChart()
        {
            if (string.IsNullOrEmpty(SelectedQuery))
            {
                await ShowPopup("Error: Please choose a query before clicking 'Add chart'.", "Red");
                return;
            }

            // Save the current state before making changes (for undo functionality)
            UndoStack.Push(new List<ViewModelBase?>(ChartSlots));
            RedoStack.Clear(); // Clear the redo stack because we're making a new change

            Console.WriteLine("Add Chart command executed.");
            Console.WriteLine($"Selected query: {SelectedQuery}");

            // Map the selected query to the corresponding ViewModel
            ViewModelBase? newChart = SelectedQuery switch
            {
                "Number of Students by School Type" => new PieChartViewModel(),
                "Number of Students by Peer Influence" => new Nr2PieChartViewModel(),
                "Average Exam Score of Students" => new Nr3PieChartViewModel(),
                "Average Exam Score by Physical Activity" => new Nr4BarChartViewModel(),
                "Average Sleep Hours of Students" => new Nr5PieChartViewModel(),
                "Motivation Level by Gender" => new Nr6PieChartViewModel(),
                _ => null
            };

            if (newChart == null) return;

            // Find the first empty slot and add the chart there
            for (int i = 0; i < ChartSlots.Count; i++)
            {
                if (ChartSlots[i] == null)
                {
                    ChartSlots[i] = newChart;

                    // Remove the selected query from the list to prevent re-adding it
                    Queries.Remove(SelectedQuery);
                    SelectedQuery = null;

                    return;
                }
            }
        }

        [RelayCommand]
        public void SelectChart(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < ChartSlots.Count && ChartSlots[slotIndex] != null)
            {
                SelectedChartIndex = slotIndex;
                DeleteButtonVisible = true; // Show the delete button
                Console.WriteLine($"Chart in slot {slotIndex + 1} selected.");
            }
            else
            {
                SelectedChartIndex = -1;
                DeleteButtonVisible = false; // Hide the delete button
                Console.WriteLine("No chart selected.");
            }
        }

        [RelayCommand]
        public void DeleteChart()
        {
            Console.WriteLine("Delete Chart command executed.");

            // Validate SelectedChartIndex
            if (SelectedChartIndex < 0 || SelectedChartIndex >= ChartSlots.Count)
            {
                Console.WriteLine("Error: SelectedChartIndex is out of range.");
                return;
            }

            // Save the current state before making changes (for undo functionality)
            UndoStack.Push(new List<ViewModelBase?>(ChartSlots));
            RedoStack.Clear(); // Clear the redo stack because we're making a new change

            var chartToRemove = ChartSlots[SelectedChartIndex];

            // Add the query back to the Queries list if it was removed
            if (chartToRemove is PieChartViewModel) Queries.Add("Number of Students by School Type");
            else if (chartToRemove is Nr2PieChartViewModel) Queries.Add("Number of Students by Peer Influence");
            else if (chartToRemove is Nr3PieChartViewModel) Queries.Add("Average Exam Score of Students");
            else if (chartToRemove is Nr4BarChartViewModel) Queries.Add("Average Exam Score by Physical Activity");
            else if (chartToRemove is Nr5PieChartViewModel) Queries.Add("Average Sleep Hours of Students");
            else if (chartToRemove is Nr6PieChartViewModel) Queries.Add("Motivation Level by Gender");

            // Shift charts left to fill empty spaces
            for (int i = SelectedChartIndex; i < ChartSlots.Count - 1; i++)
            {
                ChartSlots[i] = ChartSlots[i + 1];
            }

            // Clear the last slot
            ChartSlots[ChartSlots.Count - 1] = null;

            Console.WriteLine($"Chart in slot {SelectedChartIndex + 1} deleted and charts shifted.");

            SelectedChartIndex = -1; // Reset the selection
            DeleteButtonVisible = false;

            if (App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.UnselectBorder();
                }
            }
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
}