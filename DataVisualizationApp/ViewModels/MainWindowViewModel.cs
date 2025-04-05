using System;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;
using System.Globalization;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DataVisualizationApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly List<StudentPerformance> _data;

    public MainWindowViewModel()
    {

        _data = CsvService.LoadCsv();
        Console.WriteLine($"Loaded {_data.Count} records from CSV.");

        Queries = new List<string>
        {
            "Number of Students by School Type",
            "Number of Students by Peer Influence",
            "Average Exam Score of Students",
            "Average Exam Score by Physical Activity",
            "Average Sleep Hours of Students",
            "Motivation Level by Gender"
        };
    }

    public List<string> Queries { get; }
    [ObservableProperty] private string? selectedQuery;
    [ObservableProperty] private ViewModelBase? selectedChartViewModel;
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
            Console.WriteLine($"Selected query: {SelectedQuery}");

            // Map the selected query to the corresponding ViewModel
            SelectedChartViewModel = SelectedQuery switch
            {
                "Number of Students by School Type" => new PieChartViewModel(),
                "Number of Students by Peer Influence" => new Nr2PieChartViewModel(),
                "Average Exam Score of Students" => new Nr3PieChartViewModel(),
                "Average Exam Score by Physical Activity" => new Nr4BarChartViewModel(),
                "Average Sleep Hours of Students" => new Nr5PieChartViewModel(),
                "Motivation Level by Gender" => new Nr6PieChartViewModel(),
                _ => null
            };
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

    public void PresetQueries()
    {
        // Average Exam Score for students involved in Extracurricular Activities
        var extracurricularScores = _data
            .GroupBy(d => d.Extracurricular_Activities)
            .Select(g => new
            {
                Involved = g.Key,
                AverageScore = g.Average(d => d.Exam_Score)
            })
            .ToList();

        // Average Exam Score by Family Income
        var averageScoreByFamilyIncome = _data
            .GroupBy(d => d.Family_Income)
            .Select(g => new
            {
                FamilyIncome = g.Key,
                AverageScore = g.Average(d => d.Exam_Score)
            })
            .ToList();

        // Exam Scores of Students with Less Than 6 Hours of Sleep
        var sleepLessThan6Hours = _data
            .Where(d => d.Sleep_Hours < 6)
            .GroupBy(d => d.Sleep_Hours)
            .Select(g => new
            {
                SleepHours = g.Key,
                AverageExamScore = g.Average(d => d.Exam_Score)
            })
            .ToList();

        // Exam Scores Distribution for Students with Postgraduate Parental Education Level
        var postgraduateParentalEducation = _data
            .Where(d => d.Parental_Education_Level == "Postgraduate")
            .GroupBy(d => d.Exam_Score / 10)
            .Select(g => new
            {
                ScoreRange = g.Key * 10 + "-" + ((g.Key + 1) * 10 - 1),
                Count = g.Count()
            })
            .ToList();

        // Total Attendance by School Type
        var totalAttendanceBySchoolType = _data
            .GroupBy(d => d.School_Type)
            .Select(g => new
            {
                SchoolType = g.Key,
                TotalAttendance = g.Sum(d => d.Attendance)
            })
            .ToList();

        // Distribution of Tutoring Sessions
        var tutoringSessions = _data
            .GroupBy(d => d.Tutoring_Sessions <= 4 ? d.Tutoring_Sessions.ToString() : "5-8")
            .Select(g => new
            {
                Sessions = g.Key,
                Count = g.Count()
            })
            .ToList();

        // Min, Max, and Average Study Hours for each Motivation Level
        var studyHoursByMotivation = _data
            .GroupBy(d => d.Motivation_Level)
            .Select(g => new
            {
                MotivationLevel = g.Key,
                MinHours = g.Min(d => d.Hours_Studied),
                MaxHours = g.Max(d => d.Hours_Studied),
                AvgHours = g.Average(d => d.Hours_Studied)
            })
            .ToList();

        // Average Exam Score based on Physical Activity
        var averageScoreByPhysicalActivity = _data
            .GroupBy(d => d.Physical_Activity)
            .Select(g => new
            {
                ActivityLevel = g.Key,
                AverageScore = g.Average(d => d.Exam_Score)
            })
            .ToList();

        // Average Motivation Level based on Gender and Peer Influence
        var motivationByGenderPeerInfluence = _data
            .GroupBy(d => new { d.Gender, d.Peer_Influence })
            .Select(g => new
            {
                Gender = g.Key.Gender,
                PeerInfluence = g.Key.Peer_Influence,
                AverageMotivation = g.Average(d => double.TryParse(d.Motivation_Level, out var level) ? level : 0)
            })
            .ToList();

        // Average Exam Score by Teacher Quality
        var averageScoreByTeacherQuality = _data
            .GroupBy(d => d.Teacher_Quality)
            .Select(g => new
            {
                TeacherQuality = g.Key,
                AverageScore = g.Average(d => d.Exam_Score)
            })
            .ToList();

        // Percentage of Students by School Type
        var studentsBySchoolType = _data
            .GroupBy(d => d.School_Type)
            .Select(g => new
            {
                SchoolType = g.Key,
                Percentage = (double)g.Count() / _data.Count() * 100
            })
            .ToList();

        // Percentage of Students by Peer Influence
        var studentsByPeerInfluence = _data
            .GroupBy(d => d.Peer_Influence)
            .Select(g => new
            {
                PeerInfluence = g.Key,
                Percentage = (double)g.Count() / _data.Count() * 100
            })
            .ToList();
    }

}
