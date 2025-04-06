using Avalonia.Controls;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views
{
    public partial class Nr2PieChartView : UserControl
    {
        public Nr2PieChartView()
        {
            InitializeComponent();
            DataContext = new Nr2PieChartViewModel();
        }
    }
}