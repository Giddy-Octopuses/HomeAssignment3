using Avalonia.Controls;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views
{
    public partial class Nr3PieChartView : UserControl
    {
        public Nr3PieChartView()
        {
            InitializeComponent();
            DataContext = new Nr3PieChartViewModel();
        }
    }
}
