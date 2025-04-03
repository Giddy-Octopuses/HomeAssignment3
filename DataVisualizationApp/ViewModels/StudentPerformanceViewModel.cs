using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Measure;
using DataVisualizationApp.Models;

namespace DataVisualizationApp.ViewModels
{
    public partial class StudentPerformanceViewModel : ObservableObject
    {
        // ObservableCollection for pie chart series
        public ObservableCollection<ISeries> ExamScoreByGender { get; set; } = new();
        public ObservableCollection<ISeries> ExamScoreByIncome { get; set; } = new();

        private List<StudentPerformance> students = new();

        public StudentPerformanceViewModel()
        {
            // Load and process data immediately
            LoadData();
            ProcessData();
        }

        // Method to load data from the CSV file
        private void LoadData()
        {
            var lines = File.ReadAllLines("StudentPerformanceFactors.csv").Skip(1);
            students.Clear(); // Clear existing data to reload

            foreach (var line in lines)
            {
                var values = line.Split(',');

                students.Add(new StudentPerformance
                {
                    Hours_Studied = int.Parse(values[0]),
                    Exam_Score = int.Parse(values[^1]),
                    Gender = values[^2],
                    Family_Income = values[10],
                    Parental_Involvement = values.Length > 11 ? values[11] ?? string.Empty : string.Empty,
                    Access_to_Resources = values.Length > 12 ? values[12] ?? string.Empty : string.Empty,
                    Extracurricular_Activities = values.Length > 13 ? values[13] ?? string.Empty : string.Empty,
                    Motivation_Level = values.Length > 14 ? values[14] ?? string.Empty : string.Empty,
                    Internet_Access = values.Length > 15 ? values[15] ?? string.Empty : string.Empty,
                    Teacher_Quality = values.Length > 16 ? values[16] ?? string.Empty : string.Empty,
                    School_Type = values.Length > 17 ? values[17] ?? string.Empty : string.Empty,
                    Peer_Influence = values.Length > 18 ? values[18] ?? string.Empty : string.Empty,
                    Learning_Disabilities = values.Length > 19 ? values[19] ?? string.Empty : string.Empty,
                    Parental_Education_Level = values.Length > 20 ? values[20] ?? string.Empty : string.Empty,
                    Distance_from_Home = values.Length > 21 ? values[21] ?? string.Empty : string.Empty
                });
            }
        }

        // Method to process data for pie charts
        private void ProcessData()
        {
            // Pie chart for Exam Score by Gender
            var genderGroups = students.GroupBy(s => s.Gender)
                .Select(g => new { Gender = g.Key, Count = g.Count() })
                .ToList();

            ExamScoreByGender.Clear();
            ExamScoreByGender.Add(new PieSeries<ObservableValue>
            {
                Values = genderGroups.Select(g => new ObservableValue(g.Count)).ToList(),
                Name = "Gender Distribution",
                DataLabelsFormatter = point => $"{point.Model.Value}", // Use Model.Value instead of PrimaryValue
                DataLabelsPosition = PolarLabelsPosition.Middle // Position of the labels
            });

            // Pie chart for Exam Score by Family Income
            var incomeGroups = students.GroupBy(s => s.Family_Income)
                .Select(g => new { Income = g.Key, Count = g.Count() })
                .ToList();

            ExamScoreByIncome.Clear();
            ExamScoreByIncome.Add(new PieSeries<ObservableValue>
            {
                Values = incomeGroups.Select(g => new ObservableValue(g.Count)).ToList(),
                Name = "Income Distribution",
                DataLabelsFormatter = point => $"{point.Model.Value}", // Use Model.Value instead of PrimaryValue
                DataLabelsPosition = PolarLabelsPosition.Middle // Position of the labels
            });

            // Notify property changes to update UI bindings
            OnPropertyChanged(nameof(ExamScoreByGender));
            OnPropertyChanged(nameof(ExamScoreByIncome));
        }
    }
}