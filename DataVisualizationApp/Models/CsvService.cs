using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using CsvHelper;
using DataVisualizationApp.Models;

public class CsvService
{
    private static readonly string FilePath = "StudentPerformanceFactors.csv";

    public static List<StudentPerformance> LoadCsv(string? filePath = null)
    {
        filePath ??= FilePath;

        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"CSV file not found: {filePath}");
            }

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<StudentPerformance>().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CSV: {ex.Message}");
            return new List<StudentPerformance>(); // Return empty list if error occurs
        }
    }

}
