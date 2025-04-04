using Avalonia.Controls;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views
{
    public partial class Nr5PieChartView : UserControl
    {
        public Nr5PieChartView()
        {
            InitializeComponent();
            DataContext = new Nr5PieChartViewModel();
        }
    }
}
