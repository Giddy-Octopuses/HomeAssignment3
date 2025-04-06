using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


//Number of Students by Peer Influence  {pie chart}


namespace DataVisualizationApp.ViewModels
{
    public class Nr2PieChartViewModel : ChartViewModelBase
    {
        public Nr2PieChartViewModel()
        {
            LoadData();
        }

        protected override void LoadData()
        {
            List<StudentPerformance> students = CsvService.LoadCsv();
            var peerInfluenceCounts = students
                .GroupBy(s => s.Peer_Influence) 
                .ToDictionary(g => g.Key, g => g.Count());
            Series = peerInfluenceCounts.Select(kvp =>
                new PieSeries<int>
                {
                    Values = [kvp.Value],
                    Name = $"{kvp.Key}" 
                }).ToList();
        }
    }
}