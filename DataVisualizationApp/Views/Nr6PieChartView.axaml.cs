using Avalonia.Controls;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views
{
    public partial class Nr6PieChartView : UserControl
    {
        public Nr6PieChartView()
        {
            InitializeComponent(); 
            DataContext = new Nr6PieChartViewModel();
        }
    }
}